using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests.Services
{
    public class GamesServicesTests
    {
        private readonly Mock<IGenericEntityRepository<Game>> _repositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ILogger<GamesServices>> _loggerMock;
        private readonly GamesServices _service;

        public GamesServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Game>>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _loggerMock = new Mock<ILogger<GamesServices>>();
            _service = new GamesServices(_repositoryMock.Object, _gameRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ListarPaginado_QuandoNaoHouverJogos_DeveRetornarListaVazia()
        {
            // Arrange
            var numeroPagina = 1;
            var tamanhoPagina = 10;
            _gameRepositoryMock.Setup(r => r.ListarPaginado(numeroPagina, tamanhoPagina, null, null))
                .ReturnsAsync((new List<Game>(), 0));

            // Act
            var result = await _service.ListarPaginado(numeroPagina, tamanhoPagina, null, null);

            // Assert
            Assert.NotNull(result.Jogos);
            Assert.Empty(result.Jogos);
            Assert.Equal(0, result.TotalRegistros);
        }

        [Fact]
        public async Task ListarPaginado_QuandoHouverUmJogo_DeveRetornarListaComUmIndice()
        {
            // Arrange
            var numeroPagina = 1;
            var tamanhoPagina = 10;
            var jogos = new List<Game>
            {
                Game.Criar("Jogo Teste", "Descrição", "Ação", "Dev", 99.99m, DateTimeOffset.UtcNow)
            };
            _gameRepositoryMock.Setup(r => r.ListarPaginado(numeroPagina, tamanhoPagina, null, null))
                .ReturnsAsync((jogos, 1));

            // Act
            var result = await _service.ListarPaginado(numeroPagina, tamanhoPagina, null, null);

            // Assert
            Assert.NotNull(result.Jogos);
            Assert.NotEmpty(result.Jogos);
            Assert.Equal(1, result.Jogos.Count);
            Assert.Equal(1, result.TotalRegistros);
        }

        [Fact]
        public async Task ListarPaginado_QuandoHouverDoisJogos_DeveRetornarListaComDoisIndices()
        {
            // Arrange
            var numeroPagina = 1;
            var tamanhoPagina = 10;
            var jogos = new List<Game>
            {
                Game.Criar("Jogo Teste 1", "Descrição 1", "Ação", "Dev 1", 99.99m, DateTimeOffset.UtcNow),
                Game.Criar("Jogo Teste 2", "Descrição 2", "RPG", "Dev 2", 149.99m, DateTimeOffset.UtcNow)
            };
            _gameRepositoryMock.Setup(r => r.ListarPaginado(numeroPagina, tamanhoPagina, null, null))
                .ReturnsAsync((jogos, 2));

            // Act
            var result = await _service.ListarPaginado(numeroPagina, tamanhoPagina, null, null);

            // Assert
            Assert.NotNull(result.Jogos);
            Assert.NotEmpty(result.Jogos);
            Assert.Equal(2, result.Jogos.Count);
            Assert.Equal(2, result.TotalRegistros);
        }

        [Fact]
        public async Task AtualizarGame_QuandoGameExistir_DeveRetornarGameAtualizadoComSuccessTrue()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var gameExistente = Game.Criar("Jogo Antigo", "Descrição Antiga", "Ação", "Dev Antigo", 99.99m, DateTimeOffset.UtcNow);
            gameExistente.Id = gameId;
            var gameAtualizado = Game.Criar("Jogo Novo", "Descrição Nova", "RPG", "Dev Novo", 149.99m, DateTimeOffset.UtcNow);
            gameAtualizado.Id = gameId;
            _repositoryMock.Setup(r => r.GetById(gameId)).Returns(gameExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Game>())).Returns((gameExistente, true));

            // Act
            var result = await _service.AtualizarGame(gameAtualizado);

            // Assert
            Assert.NotNull(result.Game);
            Assert.True(result.Success);
            Assert.Equal(gameAtualizado.Nome, result.Game.Nome);
            Assert.Equal(gameAtualizado.Descricao, result.Game.Descricao);
            Assert.Equal(gameAtualizado.Genero, result.Game.Genero);
            Assert.Equal(gameAtualizado.Desenvolvedor, result.Game.Desenvolvedor);
            Assert.Equal(gameAtualizado.Preco, result.Game.Preco);
        }

        [Fact]
        public async Task AtualizarGame_QuandoGameNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var gameAtualizado = Game.Criar("Jogo Novo", "Descrição Nova", "RPG", "Dev Novo", 149.99m, DateTimeOffset.UtcNow);
            gameAtualizado.Id = gameId;
            _repositoryMock.Setup(r => r.GetById(gameId)).Returns((Game)null);

            // Act
            var result = await _service.AtualizarGame(gameAtualizado);

            // Assert
            Assert.Null(result.Game);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task AtualizarGame_QuandoAtualizacaoFalhar_DeveRetornarGameComSuccessFalse()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var gameExistente = Game.Criar("Jogo Antigo", "Descrição Antiga", "Ação", "Dev Antigo", 99.99m, DateTimeOffset.UtcNow);
            gameExistente.Id = gameId;
            var gameAtualizado = Game.Criar("Jogo Novo", "Descrição Nova", "RPG", "Dev Novo", 149.99m, DateTimeOffset.UtcNow);
            gameAtualizado.Id = gameId;
            _repositoryMock.Setup(r => r.GetById(gameId)).Returns(gameExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Game>())).Returns((gameExistente, false));

            // Act
            var result = await _service.AtualizarGame(gameAtualizado);

            // Assert
            Assert.NotNull(result.Game);
            Assert.False(result.Success);
        }
    }
}
