using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class UsuarioPerfilEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IUsuarioPerfilAppService _mockAppService;
        private readonly IValidator<CadastrarUsuarioPerfilRequest> _mockCadastrarRequestValidator;
        private readonly IValidator<AtualizarUsuarioPerfilRequest> _mockAtualizarRequestValidator;

        public UsuarioPerfilEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _mockAppService = _factory.MockUsuarioPerfilAppService;
            _mockCadastrarRequestValidator = _factory.MockCadastrarUsuarioPerfilRequestValidator;
            _mockAtualizarRequestValidator = _factory.MockAtualizarUsuarioPerfilRequestValidator;

            // Limpar os mocks antes de cada teste para garantir isolamento
            _mockAppService.ClearReceivedCalls();
            _mockCadastrarRequestValidator.ClearReceivedCalls();
            _mockAtualizarRequestValidator.ClearReceivedCalls();
        }

        // Testes para BuscarPerfilDoUsuario
        [Fact]
        public async Task BuscarPerfilPorUsuarioId_DeveRetornarOkComPerfil_QuandoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var expectedResponse = new BuscarUsuarioPerfilResponse // CORRIGIDO: Usando inicializador de objeto
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                NomeCompleto = "Usuario Teste",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-20),
                Pais = "Brasil",
                AvatarUrl = "http://example.com/avatar.png"
            };

            _mockAppService.BuscarPorUsuarioId(usuarioId).Returns(Task.FromResult(expectedResponse));

            // Act
            var response = await _client.GetAsync($"/api/usuarios/{usuarioId}/perfil/BuscarPorUsuarioId/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BuscarUsuarioPerfilResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Perfil encontrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);

            await _mockAppService.Received(1).BuscarPorUsuarioId(usuarioId);
        }

        [Fact]
        public async Task BuscarPerfilPorUsuarioId_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();

            _mockAppService.BuscarPorUsuarioId(usuarioId).Returns(Task.FromResult((BuscarUsuarioPerfilResponse?)null));

            // Act
            var response = await _client.GetAsync($"/api/usuarios/{usuarioId}/perfil/BuscarPorUsuarioId/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("perfil");
            errorResponse.Errors["perfil"].Should().Contain("Perfil não encontrado para este usuário.");

            await _mockAppService.Received(1).BuscarPorUsuarioId(usuarioId);
        }

        // Testes para CadastrarPerfil
        [Fact]
        public async Task CadastrarPerfil_DeveRetornarCreated_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = usuarioId,
                NomeCompleto = "Novo Usuario",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-25),
                Pais = "Portugal",
                AvatarUrl = "http://example.com/new_avatar.png"
            };
            var expectedResponse = new BuscarUsuarioPerfilResponse // CORRIGIDO: Usando inicializador de objeto
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                NomeCompleto = request.NomeCompleto,
                DataNascimento = request.DataNascimento,
                Pais = request.Pais,
                AvatarUrl = request.AvatarUrl
            };

            _mockCadastrarRequestValidator
                .ValidateAsync(Arg.Any<CadastrarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Cadastrar(Arg.Is<CadastrarUsuarioPerfilRequest>(r => r.UsuarioId == usuarioId))
                .Returns(Task.FromResult(expectedResponse));

            // Act
            var response = await _client.PostAsJsonAsync($"/api/usuarios/{usuarioId}/perfil/Cadastrar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BuscarUsuarioPerfilResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Perfil cadastrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);
            response.Headers.Location.Should().Be($"/api/usuarios/{usuarioId}/perfil/{expectedResponse.Id}");

            await _mockCadastrarRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Cadastrar(Arg.Is<CadastrarUsuarioPerfilRequest>(r => r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task CadastrarPerfil_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new CadastrarUsuarioPerfilRequest
            {
                UsuarioId = usuarioId,
                NomeCompleto = "", // Nome inválido para forçar falha de validação
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-25),
                Pais = "Portugal",
                AvatarUrl = "http://example.com/new_avatar.png"
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("NomeCompleto", "Nome completo é obrigatório.")
            });

            _mockCadastrarRequestValidator
                .ValidateAsync(Arg.Any<CadastrarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/usuarios/{usuarioId}/perfil/Cadastrar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("NomeCompleto");
            errorResponse.Errors["NomeCompleto"].Should().Contain("Nome completo é obrigatório.");

            await _mockCadastrarRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().Cadastrar(Arg.Any<CadastrarUsuarioPerfilRequest>());
        }


        // Testes para AtualizarPerfil
        [Fact]
        public async Task AtualizarPerfil_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfilId = Guid.NewGuid();
            var request = new AtualizarUsuarioPerfilRequest
            {
                Id = perfilId,
                UsuarioId = usuarioId,
                NomeCompleto = "Usuario Atualizado",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-22),
                Pais = "Canadá",
                AvatarUrl = "http://example.com/updated_avatar.png"
            };
            var expectedResponse = new BuscarUsuarioPerfilResponse // CORRIGIDO: Usando inicializador de objeto
            {
                Id = perfilId,
                UsuarioId = usuarioId,
                NomeCompleto = request.NomeCompleto,
                DataNascimento = request.DataNascimento,
                Pais = request.Pais,
                AvatarUrl = request.AvatarUrl
            };

            _mockAtualizarRequestValidator
                .ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Atualizar(Arg.Is<AtualizarUsuarioPerfilRequest>(r => r.Id == perfilId && r.UsuarioId == usuarioId))
                .Returns(Task.FromResult((expectedResponse, true)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/perfil/Atualizar/{perfilId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BuscarUsuarioPerfilResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Perfil atualizado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedResponse);

            await _mockAtualizarRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Atualizar(Arg.Is<AtualizarUsuarioPerfilRequest>(r => r.Id == perfilId && r.UsuarioId == usuarioId));
        }



        [Fact]
        public async Task AtualizarPerfil_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfilId = Guid.NewGuid();
            var request = new AtualizarUsuarioPerfilRequest
            {
                Id = perfilId,
                UsuarioId = usuarioId,
                NomeCompleto = "", // Nome inválido para forçar falha de validação
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-22),
                Pais = "Canadá",
                AvatarUrl = "http://example.com/updated_avatar.png"
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("NomeCompleto", "Nome completo é obrigatório.")
            });

            _mockAtualizarRequestValidator
                .ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/perfil/Atualizar/{perfilId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("NomeCompleto");
            errorResponse.Errors["NomeCompleto"].Should().Contain("Nome completo é obrigatório.");

            await _mockAtualizarRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarUsuarioPerfilRequest>());
        }

        [Fact]
        public async Task AtualizarPerfil_DeveRetornarNotFound_QuandoAppServiceRetornaFalhaOuNaoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfilId = Guid.NewGuid();
            var request = new AtualizarUsuarioPerfilRequest
            {
                Id = perfilId,
                UsuarioId = usuarioId,
                NomeCompleto = "NotFoundGamer",
                DataNascimento = DateTimeOffset.UtcNow.AddYears(-20),
                Pais = "Brasil",
                AvatarUrl = "http://example.com/notfound_avatar.png"
            };

            _mockAtualizarRequestValidator
                .ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Atualizar(Arg.Is<AtualizarUsuarioPerfilRequest>(r => r.Id == perfilId && r.UsuarioId == usuarioId))
                .Returns(Task.FromResult(((BuscarUsuarioPerfilResponse?)null, false)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/perfil/Atualizar/{perfilId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("perfil");
            errorResponse.Errors["perfil"].Should().Contain("Perfil não encontrado ou não pertence ao usuário.");

            await _mockAtualizarRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Atualizar(Arg.Is<AtualizarUsuarioPerfilRequest>(r => r.Id == perfilId && r.UsuarioId == usuarioId));
        }

        // Testes para DeletarPerfil
        [Fact]
        public async Task DeletarPerfil_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfilId = Guid.NewGuid();

            _mockAppService.Deletar(perfilId, usuarioId).Returns(Task.FromResult(true));

            // Act
            var response = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/perfil/Deletar/{perfilId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Perfil removido com sucesso.");

            await _mockAppService.Received(1).Deletar(perfilId, usuarioId);
        }

        [Fact]
        public async Task DeletarPerfil_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfilId = Guid.NewGuid();

            _mockAppService.Deletar(perfilId, usuarioId).Returns(Task.FromResult(false));

            // Act
            var response = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/perfil/Deletar/{perfilId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("perfil");
            errorResponse.Errors["perfil"].Should().Contain("Perfil não encontrado ou não pertence ao usuário.");

            await _mockAppService.Received(1).Deletar(perfilId, usuarioId);
        }
    }
}
