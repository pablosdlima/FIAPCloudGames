using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioGameBiblioteca;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class UsuarioGameBibliotecaEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IUsuarioGameBibliotecaAppService _mockAppService;
        private readonly IValidator<ComprarGameRequest> _mockComprarGameRequestValidator;
        private readonly IValidator<AtualizarBibliotecaRequest> _mockAtualizarBibliotecaRequestValidator;

        public UsuarioGameBibliotecaEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _mockAppService = _factory.MockUsuarioGameBibliotecaAppService;
            _mockComprarGameRequestValidator = _factory.MockComprarGameRequestValidator;
            _mockAtualizarBibliotecaRequestValidator = _factory.MockAtualizarBibliotecaRequestValidator;

            // Limpar os mocks antes de cada teste para garantir isolamento
            _mockAppService.ClearReceivedCalls();
            _mockComprarGameRequestValidator.ClearReceivedCalls();
            _mockAtualizarBibliotecaRequestValidator.ClearReceivedCalls();
        }

        [Fact]
        public async Task ListarBibliotecaDoUsuario_DeveRetornarOkComListaDeBibliotecas_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var expectedBibliotecas = new List<BibliotecaResponse>
            {
                new BibliotecaResponse { Id = Guid.NewGuid(), UsuarioId = usuarioId, GameId = Guid.NewGuid(), NomeGame = "Game 1" },
                new BibliotecaResponse { Id = Guid.NewGuid(), UsuarioId = usuarioId, GameId = Guid.NewGuid(), NomeGame = "Game 2" }
            };

            _mockAppService.ListarPorUsuario(usuarioId).Returns(Task.FromResult(expectedBibliotecas));

            // Act
            var response = await _client.GetAsync($"/api/usuarios/{usuarioId}/biblioteca/BuscarPorUsuarioId/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BibliotecaResponse>>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Biblioteca listada com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedBibliotecas);

            await _mockAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task ListarBibliotecaDoUsuario_DeveRetornarOkComListaVazia_QuandoUsuarioNaoPossuiJogos()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            _mockAppService.ListarPorUsuario(usuarioId).Returns(Task.FromResult(new List<BibliotecaResponse>()));

            // Act
            var response = await _client.GetAsync($"/api/usuarios/{usuarioId}/biblioteca/BuscarPorUsuarioId/");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BibliotecaResponse>>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Biblioteca listada com sucesso.");
            apiResponse.Data.Should().BeEmpty();

            await _mockAppService.Received(1).ListarPorUsuario(usuarioId);
        }

        [Fact]
        public async Task ComprarGame_DeveRetornarCreated_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new ComprarGameRequest
            {
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Digital",
                PrecoAquisicao = 59.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var expectedBibliotecaResponse = new BibliotecaResponse
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                GameId = request.GameId,
                TipoAquisicao = request.TipoAquisicao,
                PrecoAquisicao = request.PrecoAquisicao,
                DataAquisicao = request.DataAquisicao
            };

            _mockComprarGameRequestValidator
                .ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.ComprarGame(Arg.Is<ComprarGameRequest>(r => r.UsuarioId == usuarioId && r.GameId == request.GameId))
                .Returns(Task.FromResult((expectedBibliotecaResponse, true, (string?)null)));

            // Act
            var response = await _client.PostAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Comprar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BibliotecaResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogo comprado e adicionado à biblioteca com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedBibliotecaResponse);
            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location!.OriginalString.Should().Contain($"/api/usuarios/{usuarioId}/biblioteca/{expectedBibliotecaResponse.Id}");

            await _mockComprarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).ComprarGame(Arg.Is<ComprarGameRequest>(r => r.UsuarioId == usuarioId && r.GameId == request.GameId));
        }

        [Fact]
        public async Task ComprarGame_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new ComprarGameRequest
            {
                GameId = Guid.Empty, // ID inválido para forçar falha de validação
                TipoAquisicao = "Digital",
                PrecoAquisicao = 59.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("GameId", "GameId é obrigatório.")
            });

            _mockComprarGameRequestValidator
                .ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PostAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Comprar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest); // CORRIGIDO: Usando StatusCode
            errorResponse.Errors.Should().ContainKey("GameId");
            errorResponse.Errors["GameId"].Should().Contain("GameId é obrigatório.");

            await _mockComprarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().ComprarGame(Arg.Any<ComprarGameRequest>());
        }

        [Fact]
        public async Task ComprarGame_DeveRetornarBadRequest_QuandoAppServiceRetornaFalha()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new ComprarGameRequest
            {
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Digital",
                PrecoAquisicao = 59.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var errorMessage = "Você já possui este jogo na sua biblioteca.";

            _mockComprarGameRequestValidator
                .ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.ComprarGame(Arg.Is<ComprarGameRequest>(r => r.UsuarioId == usuarioId && r.GameId == request.GameId))
                .Returns(Task.FromResult(((BibliotecaResponse?)null, false, errorMessage)));

            // Act
            var response = await _client.PostAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Comprar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("game");
            errorResponse.Errors["game"].Should().Contain(errorMessage);

            await _mockComprarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).ComprarGame(Arg.Is<ComprarGameRequest>(r => r.UsuarioId == usuarioId && r.GameId == request.GameId));
        }

        [Fact]
        public async Task AtualizarBiblioteca_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var bibliotecaId = Guid.NewGuid();
            var request = new AtualizarBibliotecaRequest
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Fisica",
                PrecoAquisicao = 79.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var expectedBibliotecaResponse = new BibliotecaResponse
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = request.GameId,
                TipoAquisicao = request.TipoAquisicao,
                PrecoAquisicao = request.PrecoAquisicao,
                DataAquisicao = request.DataAquisicao
            };

            _mockAtualizarBibliotecaRequestValidator
                .ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Atualizar(Arg.Is<AtualizarBibliotecaRequest>(r => r.Id == bibliotecaId && r.UsuarioId == usuarioId))
                .Returns(Task.FromResult((expectedBibliotecaResponse, true)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Atualizar/{bibliotecaId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BibliotecaResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Item da biblioteca atualizado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedBibliotecaResponse);

            await _mockAtualizarBibliotecaRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Atualizar(Arg.Is<AtualizarBibliotecaRequest>(r => r.Id == bibliotecaId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task AtualizarBiblioteca_DeveRetornarBadRequest_QuandoIdDaUrlNaoCorrespondeAoIdDoCorpo()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var urlBibliotecaId = Guid.NewGuid();
            var request = new AtualizarBibliotecaRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Fisica",
                PrecoAquisicao = 79.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };

            _mockAtualizarBibliotecaRequestValidator
                .ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Atualizar/{urlBibliotecaId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("id");
            errorResponse.Errors["id"].Should().Contain("Id da URL não corresponde ao Id do corpo da requisição.");

            await _mockAtualizarBibliotecaRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarBibliotecaRequest>());
        }


        [Fact]
        public async Task AtualizarBiblioteca_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var bibliotecaId = Guid.NewGuid();
            var request = new AtualizarBibliotecaRequest
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.Empty,
                TipoAquisicao = "Fisica",
                PrecoAquisicao = 79.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("GameId", "GameId é obrigatório.")
            });

            _mockAtualizarBibliotecaRequestValidator
                .ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Atualizar/{bibliotecaId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("GameId");
            errorResponse.Errors["GameId"].Should().Contain("GameId é obrigatório.");

            await _mockAtualizarBibliotecaRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.DidNotReceive().Atualizar(Arg.Any<AtualizarBibliotecaRequest>());
        }

        [Fact]
        public async Task AtualizarBiblioteca_DeveRetornarNotFound_QuandoAppServiceRetornaFalhaOuNaoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var bibliotecaId = Guid.NewGuid();
            var request = new AtualizarBibliotecaRequest
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Fisica",
                PrecoAquisicao = 79.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };

            _mockAtualizarBibliotecaRequestValidator
                .ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockAppService.Atualizar(Arg.Is<AtualizarBibliotecaRequest>(r => r.Id == bibliotecaId && r.UsuarioId == usuarioId))
                .Returns(Task.FromResult(((BibliotecaResponse?)null, false)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usuarios/{usuarioId}/biblioteca/Atualizar/{bibliotecaId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("biblioteca");
            errorResponse.Errors["biblioteca"].Should().Contain("Item da biblioteca não encontrado ou não pertence ao usuário.");

            await _mockAtualizarBibliotecaRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>());
            await _mockAppService.Received(1).Atualizar(Arg.Is<AtualizarBibliotecaRequest>(r => r.Id == bibliotecaId && r.UsuarioId == usuarioId));
        }

        [Fact]
        public async Task DeletarDaBiblioteca_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var bibliotecaId = Guid.NewGuid();

            _mockAppService.Deletar(bibliotecaId, usuarioId).Returns(Task.FromResult(true));

            // Act
            var response = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/biblioteca/Deletar/{bibliotecaId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogo removido da biblioteca com sucesso.");

            await _mockAppService.Received(1).Deletar(bibliotecaId, usuarioId);
        }

        [Fact]
        public async Task DeletarDaBiblioteca_DeveRetornarNotFound_QuandoItemNaoEncontrado()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var bibliotecaId = Guid.NewGuid();

            _mockAppService.Deletar(bibliotecaId, usuarioId).Returns(Task.FromResult(false));

            // Act
            var response = await _client.DeleteAsync($"/api/usuarios/{usuarioId}/biblioteca/Deletar/{bibliotecaId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("biblioteca");
            errorResponse.Errors["biblioteca"].Should().Contain("Item da biblioteca não encontrado ou não pertence ao usuário.");

            await _mockAppService.Received(1).Deletar(bibliotecaId, usuarioId);
        }
    }
}
