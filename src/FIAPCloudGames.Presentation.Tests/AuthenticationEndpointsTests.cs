using Bogus;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Dtos.Responses.Authentication;
using FIAPCloudGames.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ClearExtensions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class AuthenticationEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IAuthenticationAppService _mockAuthenticationAppService;
        private readonly IValidator<LoginRequest> _mockLoginRequestValidator;
        private readonly ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService> _mockAppServiceLogger;

        public AuthenticationEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _mockAuthenticationAppService = _factory.MockAuthenticationAppService;
            _mockLoginRequestValidator = _factory.MockLoginRequestValidator;
            _mockAppServiceLogger = _factory.MockAppServiceLogger;

            _mockAuthenticationAppService.ClearSubstitute();
            _mockLoginRequestValidator.ClearSubstitute();
            _mockAppServiceLogger.ClearSubstitute();
        }

        private static StringContent ObterConteudoJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }

        private async Task<T?> DeserializarResposta<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }

            if (typeof(T) == typeof(object))
            {
                var jsonNode = JsonNode.Parse(jsonString);
                return (T)(object)jsonNode!;
            }

            return JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_RetornaOkComToken()
        {
            var requisicao = new Faker<LoginRequest>()
                .RuleFor(r => r.Usuario, f => f.Internet.UserName())
                .RuleFor(r => r.Senha, f => f.Internet.Password())
                .Generate();

            var tokenEsperado = new Faker().Random.Hash();
            var respostaLogin = new LoginResponse(tokenEsperado);

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(respostaLogin);

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            dynamic conteudoResposta = await DeserializarResposta<object>(resposta);

            int statusCode = conteudoResposta!["statusCode"].GetValue<int>();
            statusCode.Should().Be(200);

            string mensagem = conteudoResposta!["message"].GetValue<string>();
            mensagem.Should().Be("Login realizado com sucesso.");

            string token = conteudoResposta!["data"]!["token"].GetValue<string>();
            token.Should().Be(tokenEsperado);

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_RetornaNaoAutorizado()
        {
            var requisicao = new Faker<LoginRequest>()
                .RuleFor(r => r.Usuario, f => f.Internet.UserName())
                .RuleFor(r => r.Senha, f => f.Internet.Password())
                .Generate();

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(Task.FromException<LoginResponse>(new AutenticacaoException("Usuário ou senha inválidos.")));

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            dynamic conteudoResposta = await DeserializarResposta<object>(resposta);

            int statusCode = conteudoResposta!["statusCode"].GetValue<int>();
            statusCode.Should().Be(401);

            string mensagem = conteudoResposta!["message"].GetValue<string>();
            mensagem.Should().Be("Authentication failed");

            var noErros = conteudoResposta!["errors"];
            var errosCredenciais = ((JsonArray)noErros!["credenciais"]!).Select((Func<JsonNode, string>)(n => n!.GetValue<string>())).ToArray();
            errosCredenciais.Should().Contain("Usuário ou senha inválidos.");

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComExcecaoNaoTratada_RetornaProblema()
        {
            var requisicao = new Faker<LoginRequest>()
                .RuleFor(r => r.Usuario, f => f.Internet.UserName())
                .RuleFor(r => r.Senha, f => f.Internet.Password())
                .Generate();

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(Task.FromException<LoginResponse>(new Exception("Algo deu muito errado!")));

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var detalhesProblema = await DeserializarResposta<Microsoft.AspNetCore.Mvc.ProblemDetails>(resposta);
            detalhesProblema!.Detail.Should().Be("Ocorreu um erro inesperado durante o login.");

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComRequisicaoInvalida_RetornaBadRequestDoFiltroDeValidacao()
        {
            var requisicaoInvalida = new LoginRequest { Usuario = "", Senha = "" };

            var errosValidacao = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Usuario", "Usuario é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Senha", "Senha é obrigatório")
            };

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Is<LoginRequest>(r => r.Usuario == requisicaoInvalida.Usuario && r.Senha == requisicaoInvalida.Senha), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(errosValidacao));

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Is<LoginRequest>(r => r.Usuario != requisicaoInvalida.Usuario || r.Senha != requisicaoInvalida.Senha), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicaoInvalida));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            dynamic conteudoResposta = await DeserializarResposta<object>(resposta);

            int statusCode = conteudoResposta!["statusCode"].GetValue<int>();
            statusCode.Should().Be(400);

            string mensagem = conteudoResposta!["message"].GetValue<string>();
            mensagem.Should().Be("Erro de validação");

            var noErros = conteudoResposta!["errors"];
            var errosUsuario = ((JsonArray)noErros!["Usuario"]!).Select((Func<JsonNode, string>)(n => n!.GetValue<string>())).ToArray();
            errosUsuario.Should().Contain("Usuario é obrigatório");

            var errosSenha = ((JsonArray)noErros!["Senha"]!).Select((Func<JsonNode, string>)(n => n!.GetValue<string>())).ToArray();
            errosSenha.Should().Contain("Senha é obrigatório");

            await _mockAuthenticationAppService.DidNotReceive().Login(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
