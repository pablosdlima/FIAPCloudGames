using FIAPCloudGames.Application.Common.Models;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;
using FIAPCloudGames.Domain.Models;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class GameEndpointsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IGameAppService _mockGameAppService;
        private readonly IValidator<CadastrarGameRequest> _mockCadastrarGameRequestValidator;
        private readonly IValidator<ListarGamesPaginadoRequest> _mockListarGamesPaginadoRequestValidator;
        private readonly IValidator<AtualizarGameRequest> _mockAtualizarGameRequestValidator;

        public GameEndpointsTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _mockGameAppService = _factory.MockGameAppService;
            _mockCadastrarGameRequestValidator = _factory.MockCadastrarGameRequestValidator;
            _mockListarGamesPaginadoRequestValidator = _factory.MockListarGamesPaginadoRequestValidator;
            _mockAtualizarGameRequestValidator = _factory.MockAtualizarGameRequestValidator;

            // Limpar os mocks antes de cada teste para garantir isolamento
            _mockGameAppService.ClearReceivedCalls();
            _mockCadastrarGameRequestValidator.ClearReceivedCalls();
            _mockListarGamesPaginadoRequestValidator.ClearReceivedCalls();
            _mockAtualizarGameRequestValidator.ClearReceivedCalls();
        }

        [Fact]
        public async Task CadastrarGame_DeveRetornarCreated_QuandoSucesso()
        {
            // Arrange
            var request = new CadastrarGameRequest
            {
                Nome = "The Legend of Zelda: Breath of the Wild",
                Descricao = "Um jogo de aventura de mundo aberto.",
                Genero = "Aventura",
                Desenvolvedor = "Nintendo",
                DataRelease = DateTime.UtcNow.AddYears(-5),
                Preco = 299.99m
            };
            var expectedGame = Game.Criar(request.Nome, request.Descricao, request.Genero, request.Desenvolvedor, request.Preco, request.DataRelease);
            var expectedGameResponse = new GameResponse
            {
                Id = expectedGame.Id,
                Nome = expectedGame.Nome,
                Descricao = expectedGame.Descricao,
                Genero = expectedGame.Genero,
                Desenvolvedor = expectedGame.Desenvolvedor,
                DataRelease = expectedGame.DataRelease,
                Preco = expectedGame.Preco
            };

            _mockCadastrarGameRequestValidator
                .ValidateAsync(Arg.Any<CadastrarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockGameAppService.Cadastrar(Arg.Is<CadastrarGameRequest>(r => r.Nome == request.Nome))
                .Returns(Task.FromResult(expectedGame));

            // Act
            var response = await _client.PostAsJsonAsync("/api/Game/Cadastrar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GameResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogo cadastrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedGameResponse, options => options.Excluding(g => g.DataRelease));

            await _mockCadastrarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.Received(1).Cadastrar(Arg.Is<CadastrarGameRequest>(r => r.Nome == request.Nome));
        }

        [Fact]
        public async Task CadastrarGame_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var request = new CadastrarGameRequest
            {
                Nome = "",
                Descricao = "Um jogo de aventura de mundo aberto.",
                Genero = "Aventura",
                Desenvolvedor = "Nintendo",
                DataRelease = DateTime.UtcNow.AddYears(-5),
                Preco = 299.99m
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório.")
            });

            _mockCadastrarGameRequestValidator
                .ValidateAsync(Arg.Any<CadastrarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PostAsJsonAsync("/api/Game/Cadastrar/", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("Nome");
            errorResponse.Errors["Nome"].Should().Contain("Nome é obrigatório.");

            await _mockCadastrarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<CadastrarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.DidNotReceive().Cadastrar(Arg.Any<CadastrarGameRequest>());
        }


        [Fact]
        public async Task BuscarGamePorId_DeveRetornarOkComGame_QuandoEncontrado()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var expectedGame = Game.Criar("Game Teste", "Desc Teste", "Acao", "Dev Teste", 60.00m, DateTimeOffset.UtcNow);
            expectedGame.Id = gameId;
            var expectedGameResponse = new GameResponse
            {
                Id = expectedGame.Id,
                Nome = expectedGame.Nome,
                Descricao = expectedGame.Descricao,
                Genero = expectedGame.Genero,
                Desenvolvedor = expectedGame.Desenvolvedor,
                DataRelease = expectedGame.DataRelease,
                Preco = expectedGame.Preco
            };

            _mockGameAppService.BuscarPorId(gameId).Returns(expectedGame);

            // Act
            var response = await _client.GetAsync($"/api/Game/BuscarPorId/{gameId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GameResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogo encontrado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedGameResponse, options => options.Excluding(g => g.DataRelease));

            _mockGameAppService.Received(1).BuscarPorId(gameId);
        }

        [Fact]
        public async Task BuscarGamePorId_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            _mockGameAppService.BuscarPorId(gameId).Returns((Game?)null);

            // Act
            var response = await _client.GetAsync($"/api/Game/BuscarPorId/{gameId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("game");
            errorResponse.Errors["game"].Should().Contain("Jogo não encontrado.");

            _mockGameAppService.Received(1).BuscarPorId(gameId);
        }


        [Fact]
        public async Task ListarGamesPaginado_DeveRetornarOkComListaPaginada_QuandoSucesso()
        {
            // Arrange
            var request = new ListarGamesPaginadoRequest { NumeroPagina = 1, TamanhoPagina = 2 };
            var expectedGames = new List<GameResponse>
            {
                new GameResponse { Id = Guid.NewGuid(), Nome = "Game A", Descricao = "Desc A", Genero = "Acao", Desenvolvedor = "Dev A", DataRelease = DateTimeOffset.UtcNow.AddYears(-1), Preco = 50.00m },
                new GameResponse { Id = Guid.NewGuid(), Nome = "Game B", Descricao = "Desc B", Genero = "RPG", Desenvolvedor = "Dev B", DataRelease = DateTimeOffset.UtcNow.AddYears(-2), Preco = 70.00m }
            };
            var expectedResponse = new ListarGamesPaginadoResponse
            {
                PaginaAtual = request.NumeroPagina,
                TamanhoPagina = request.TamanhoPagina,
                TotalRegistros = 2,
                TotalPaginas = 1,
                TemPaginaAnterior = false,
                TemProximaPagina = false,
                Jogos = expectedGames
            };

            _mockListarGamesPaginadoRequestValidator
                .ValidateAsync(Arg.Any<ListarGamesPaginadoRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockGameAppService.ListarGamesPaginado(Arg.Is<ListarGamesPaginadoRequest>(r => r.NumeroPagina == request.NumeroPagina))
                .Returns(Task.FromResult(expectedResponse));

            // Act
            var response = await _client.GetAsync($"/api/Game/ListarGames?NumeroPagina={request.NumeroPagina}&TamanhoPagina={request.TamanhoPagina}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<ListarGamesPaginadoResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogos listados com sucesso.");

            apiResponse.Data.Should().BeEquivalentTo(expectedResponse, options => options
                .Excluding(ctx => ctx.Path.EndsWith("DataRelease"))
            );

            await _mockListarGamesPaginadoRequestValidator.Received(1).ValidateAsync(Arg.Any<ListarGamesPaginadoRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.Received(1).ListarGamesPaginado(Arg.Is<ListarGamesPaginadoRequest>(r => r.NumeroPagina == request.NumeroPagina));
        }

        [Fact]
        public async Task ListarGamesPaginado_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var request = new ListarGamesPaginadoRequest { NumeroPagina = 0, TamanhoPagina = 10 };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("NumeroPagina", "Número da página deve ser maior que zero.")
            });

            _mockListarGamesPaginadoRequestValidator
                .ValidateAsync(Arg.Any<ListarGamesPaginadoRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.GetAsync($"/api/Game/ListarGames?NumeroPagina={request.NumeroPagina}&TamanhoPagina={request.TamanhoPagina}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("NumeroPagina");
            errorResponse.Errors["NumeroPagina"].Should().Contain("Número da página deve ser maior que zero.");

            await _mockListarGamesPaginadoRequestValidator.Received(1).ValidateAsync(Arg.Any<ListarGamesPaginadoRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.DidNotReceive().ListarGamesPaginado(Arg.Any<ListarGamesPaginadoRequest>());
        }

        [Fact]
        public async Task AtualizarGame_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var request = new AtualizarGameRequest
            {
                Id = gameId,
                Nome = "The Witcher 3: Wild Hunt (GOTY)",
                Descricao = "RPG de mundo aberto com expansões",
                Genero = "RPG",
                Desenvolvedor = "CD Projekt Red",
                DataRelease = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
                Preco = 150.00m
            };
            var expectedGameResponse = new AtualizarGameResponse
            {
                Id = gameId,
                Nome = request.Nome,
                Descricao = request.Descricao,
                Genero = request.Genero,
                Desenvolvedor = request.Desenvolvedor,
                DataRelease = new DateTimeOffset(request.DataRelease!.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero),
                Preco = request.Preco,
                DataCriacao = DateTimeOffset.UtcNow.AddYears(-6)
            };

            _mockAtualizarGameRequestValidator
                .ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockGameAppService.AtualizarGame(Arg.Is<AtualizarGameRequest>(r => r.Id == gameId))
                .Returns(Task.FromResult((expectedGameResponse, true)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Game/AtualizarGame/{gameId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AtualizarGameResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Message.Should().Be("Jogo atualizado com sucesso.");
            apiResponse.Data.Should().BeEquivalentTo(expectedGameResponse, options => options.Excluding(g => g.DataRelease));

            await _mockAtualizarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.Received(1).AtualizarGame(Arg.Is<AtualizarGameRequest>(r => r.Id == gameId));
        }


        [Fact]
        public async Task AtualizarGame_DeveRetornarBadRequest_QuandoValidacaoFalha()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var request = new AtualizarGameRequest
            {
                Id = gameId,
                Nome = "",
                Descricao = "RPG de mundo aberto",
                Genero = "RPG",
                Desenvolvedor = "CD Projekt Red",
                DataRelease = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
                Preco = 120.00m
            };
            var validationResult = new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Nome", "Nome é obrigatório.")
            });

            _mockAtualizarGameRequestValidator
                .ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(validationResult);

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Game/AtualizarGame/{gameId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            errorResponse.Errors.Should().ContainKey("Nome");
            errorResponse.Errors["Nome"].Should().Contain("Nome é obrigatório.");

            await _mockAtualizarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.DidNotReceive().AtualizarGame(Arg.Any<AtualizarGameRequest>());
        }

        [Fact]
        public async Task AtualizarGame_DeveRetornarNotFound_QuandoAppServiceRetornaFalhaOuNaoEncontrado()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var request = new AtualizarGameRequest
            {
                Id = gameId,
                Nome = "The Witcher 3",
                Descricao = "RPG de mundo aberto",
                Genero = "RPG",
                Desenvolvedor = "CD Projekt Red",
                DataRelease = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
                Preco = 120.00m
            };

            _mockAtualizarGameRequestValidator
                .ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>())
                .Returns(new FluentValidation.Results.ValidationResult());

            _mockGameAppService.AtualizarGame(Arg.Is<AtualizarGameRequest>(r => r.Id == gameId))
                .Returns(Task.FromResult(((AtualizarGameResponse?)null, false)));

            // Act
            var response = await _client.PutAsJsonAsync($"/api/Game/AtualizarGame/{gameId}", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorDetails>();
            errorResponse.Should().NotBeNull();
            errorResponse!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            errorResponse.Errors.Should().ContainKey("game");
            errorResponse.Errors["game"].Should().Contain("Jogo não encontrado ou não foi possível atualizar.");

            await _mockAtualizarGameRequestValidator.Received(1).ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>());
            await _mockGameAppService.Received(1).AtualizarGame(Arg.Is<AtualizarGameRequest>(r => r.Id == gameId));
        }
    }
}
