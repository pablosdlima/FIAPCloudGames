using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class UsuariosEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IUsuarioAppService _mockAppService;
        private readonly IValidator<CadastrarUsuarioRequest> _mockCadastrarRequestValidator;
        private readonly IValidator<AlterarSenhaRequest> _mockAlterarSenhaRequestValidator;

        public UsuariosEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _mockAppService = _factory.MockUsuarioAppService;
            _mockCadastrarRequestValidator = _factory.MockCadastrarUsuarioRequestValidator;
            _mockAlterarSenhaRequestValidator = _factory.MockAlterarSenhaRequestValidator;

            _mockAppService.ClearReceivedCalls();
            _mockCadastrarRequestValidator.ClearReceivedCalls();
            _mockAlterarSenhaRequestValidator.ClearReceivedCalls();
        }

        [Fact]
        public async Task BuscarUsuarioPorId_DeveRetornarOkComUsuario_QuandoEncontrado()
        {
            var usuarioId = Guid.NewGuid();
            var expectedResponse = new BuscarPorIdResponse
            {
                Id = usuarioId,
                Nome = "Teste",
                Ativo = true,
                DataCriacao = DateTimeOffset.UtcNow,
                DataAtualizacao = DateTimeOffset.UtcNow,
                Perfil = new UsuarioPerfilResponse
                {
                    Id = Guid.NewGuid(),
                    NomeCompleto = "Nome Completo Teste",
                    DataNascimento = DateTimeOffset.UtcNow.AddYears(-20),
                    Pais = "Brasil",
                    AvatarUrl = "http://example.com/avatar.png"
                },
                Roles =
                [
                    new UsuarioRoleResponse
                    {
                        Id = Guid.NewGuid(),
                        RoleId = 1,
                        RoleName = "Admin",
                        Description = "Administrador do sistema"
                    }
                ]
            };

            _mockAppService.BuscarPorId(usuarioId).Returns(expectedResponse);

            var response = await _client.GetAsync($"/api/Usuarios/BuscarPorId/{usuarioId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BuscarPorIdResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Usuário encontrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);

            _mockAppService.Received(1).BuscarPorId(usuarioId);
        }

        [Fact]
        public async Task BuscarUsuarioPorId_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            var usuarioId = Guid.NewGuid();

            _mockAppService.BuscarPorId(usuarioId).Returns(x => throw new KeyNotFoundException());

            var response = await _client.GetAsync($"/api/Usuarios/BuscarPorId/{usuarioId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("usuario");
            errorResponse.Errors["usuario"].Should().Contain("Usuário não encontrado.");

            _mockAppService.Received(1).BuscarPorId(usuarioId);
        }

        [Fact]
        public async Task CadastrarUsuario_DeveRetornarCreated_QuandoSucesso()
        {
            var request = new CadastrarUsuarioRequest
            {
                Nome = "Novo Usuário",
                Senha = "SenhaSegura123",
                Celular = "11987654321",
                Email = "novo.usuario@example.com",
                TipoUsuario = TipoUsuario.Usuario,
                NomeCompleto = "Nome Completo do Novo Usuário",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-25),
                Pais = "Brasil",
                AvatarUrl = "http://example.com/new_avatar.png"
            };
            var expectedResponse = new CadastrarUsuarioResponse { IdUsuario = Guid.NewGuid() };

            _mockCadastrarRequestValidator
                .ValidateAsync(Arg.Any<CadastrarUsuarioRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Cadastrar(Arg.Is<CadastrarUsuarioRequest>(r => r.Email == request.Email))
                .Returns(Task.FromResult(expectedResponse));

            var response = await _client.PostAsJsonAsync("/api/Usuarios/Cadastrar/", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<CadastrarUsuarioResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Usuário cadastrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);
            response.Headers.Location.Should().Be($"/api/Usuarios/{expectedResponse.IdUsuario}");

            await _mockCadastrarRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarUsuarioRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Cadastrar(Arg.Is<CadastrarUsuarioRequest>(r => r.Email == request.Email));
        }

        [Fact]
        public async Task CadastrarUsuario_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            var request = new CadastrarUsuarioRequest
            {
                Nome = "Novo Usuário",
                Senha = "SenhaSegura123",
                Celular = "11987654321",
                Email = "emailinvalido",
                TipoUsuario = TipoUsuario.Usuario,
                NomeCompleto = "Nome Completo do Novo Usuário",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-25),
                Pais = "Brasil",
                AvatarUrl = "http://example.com/new_avatar.png"
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Email", "Email inválido.")
            });

            _mockCadastrarRequestValidator
                .ValidateAsync(Arg.Any<CadastrarUsuarioRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            var response = await _client.PostAsJsonAsync("/api/Usuarios/Cadastrar/", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("Email");
            errorResponse.Errors["Email"].Should().Contain("Email inválido.");

            await _mockCadastrarRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarUsuarioRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().Cadastrar(Arg.Any<CadastrarUsuarioRequest>());
        }

        [Fact]
        public async Task AlterarSenha_DeveRetornarOk_QuandoSucesso()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), "NovaSenha123");

            _mockAlterarSenhaRequestValidator
                .ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.AlterarSenha(Arg.Is<AlterarSenhaRequest>(r => r.Id == request.Id))
                .Returns(Task.FromResult(true));

            var response = await _client.PutAsJsonAsync("/api/Usuarios/AlterarSenha/", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Senha alterada com sucesso.");

            await _mockAlterarSenhaRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).AlterarSenha(Arg.Is<AlterarSenhaRequest>(r => r.Id == request.Id));
        }

        [Fact]
        public async Task AlterarSenha_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), "123"); // Senha muito curta

            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Senha", "Senha deve ter no mínimo 6 caracteres.")
            });

            _mockAlterarSenhaRequestValidator
                .ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            var response = await _client.PutAsJsonAsync("/api/Usuarios/AlterarSenha/", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("Senha");
            errorResponse.Errors["Senha"].Should().Contain("Senha deve ter no mínimo 6 caracteres.");

            await _mockAlterarSenhaRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().AlterarSenha(Arg.Any<AlterarSenhaRequest>());
        }

        [Fact]
        public async Task AlterarSenha_DeveRetornarNotFound_QuandoAppServiceRetornaFalha()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), "SenhaIncorreta");

            _mockAlterarSenhaRequestValidator
                .ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.AlterarSenha(Arg.Is<AlterarSenhaRequest>(r => r.Id == request.Id))
                .Returns(Task.FromResult(false));

            var response = await _client.PutAsJsonAsync("/api/Usuarios/AlterarSenha/", request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("usuario");
            errorResponse.Errors["usuario"].Should().Contain("Usuário não encontrado ou senha atual incorreta.");

            await _mockAlterarSenhaRequestValidator.Received(1).ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).AlterarSenha(Arg.Is<AlterarSenhaRequest>(r => r.Id == request.Id));
        }
    }
}
