using Bogus;
using FIAPCloudGames.Application.Common.Models;
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

        private async Task<ApiResponse<T>?> DeserializarRespostaSucesso<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }
            return JsonSerializer.Deserialize<ApiResponse<T>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<ErrorDetails?> DeserializarRespostaErro(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonString))
            {
                return default;
            }
            return JsonSerializer.Deserialize<ErrorDetails>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
            var mensagemSucesso = "Login realizado com sucesso.";

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(respostaLogin);

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudoResposta = await DeserializarRespostaSucesso<LoginResponse>(resposta);

            conteudoResposta.Should().NotBeNull();
            conteudoResposta!.Success.Should().BeTrue();
            conteudoResposta.Message.Should().Be(mensagemSucesso);
            conteudoResposta.Data.Should().NotBeNull();
            conteudoResposta.Data!.Token.Should().Be(tokenEsperado);
            conteudoResposta.Errors.Should().BeNull();

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_RetornaUnauthorized()
        {
            var requisicao = new Faker<LoginRequest>()
                .RuleFor(r => r.Usuario, f => f.Internet.UserName())
                .RuleFor(r => r.Senha, f => f.Internet.Password())
                .Generate();
            var mensagemErro = "Usuário ou senha inválidos.";

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(Task.FromException<LoginResponse>(new AutenticacaoException(mensagemErro)));

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(401);
            conteudoRespostaErro.Message.Should().Be(mensagemErro);
            conteudoRespostaErro.Errors.Should().BeNull();

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComExcecaoNaoTratada_RetornaProblem()
        {
            var requisicao = new Faker<LoginRequest>()
                .RuleFor(r => r.Usuario, f => f.Internet.UserName())
                .RuleFor(r => r.Senha, f => f.Internet.Password())
                .Generate();
            var mensagemErroInterno = "Ocorreu um erro interno no servidor";

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAuthenticationAppService.Login(requisicao.Usuario, requisicao.Senha)
                .Returns(Task.FromException<LoginResponse>(new Exception("Erro inesperado no serviço de autenticação.")));

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(requisicao));

            resposta.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(500);
            conteudoRespostaErro.Message.Should().Be(mensagemErroInterno);
            conteudoRespostaErro.Errors.Should().BeNull();

            await _mockAuthenticationAppService.Received(1).Login(requisicao.Usuario, requisicao.Senha);
        }

        [Fact]
        public async Task Login_ComRequisicaoInvalida_RetornaBadRequestDoFiltroDeValidacao()
        {
            var invalidRequest = new LoginRequest
            {
                Usuario = string.Empty,
                Senha = string.Empty
            };
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Usuario", "Usuario é obrigatório"),
                new FluentValidation.Results.ValidationFailure("Senha", "Senha é obrigatório")
            };
            var mensagemErroValidacao = "Erro de validação";

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Is<LoginRequest>(r => r.Usuario == invalidRequest.Usuario && r.Senha == invalidRequest.Senha), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult(validationErrors));

            _mockLoginRequestValidator
                .ValidateAsync(Arg.Is<LoginRequest>(r => r.Usuario != invalidRequest.Usuario || r.Senha != invalidRequest.Senha), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            var resposta = await _client.PostAsync("/api/Authentication/login/", ObterConteudoJson(invalidRequest));

            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var conteudoRespostaErro = await DeserializarRespostaErro(resposta);

            conteudoRespostaErro.Should().NotBeNull();
            conteudoRespostaErro!.StatusCode.Should().Be(400);
            conteudoRespostaErro.Message.Should().Be(mensagemErroValidacao);
            conteudoRespostaErro.Errors.Should().NotBeNull();
            conteudoRespostaErro.Errors!.Should().ContainKey("Usuario");
            conteudoRespostaErro.Errors!["Usuario"].Should().Contain("Usuario é obrigatório");
            conteudoRespostaErro.Errors!.Should().ContainKey("Senha");
            conteudoRespostaErro.Errors!["Senha"].Should().Contain("Senha é obrigatório");

            await _mockAuthenticationAppService.DidNotReceive().Login(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
