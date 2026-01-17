using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests.Services
{
    public class UsuarioGameBibliotecaServicesTests
    {
        private readonly Mock<IGenericEntityRepository<UsuarioGameBiblioteca>> _repositoryMock;
        private readonly Mock<IUsuarioGameBibliotecaRepository> _bibliotecaRepositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ILogger<UsuarioGameBibliotecaServices>> _loggerMock;
        private readonly UsuarioGameBibliotecaServices _service;

        public UsuarioGameBibliotecaServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<UsuarioGameBiblioteca>>();
            _bibliotecaRepositoryMock = new Mock<IUsuarioGameBibliotecaRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _loggerMock = new Mock<ILogger<UsuarioGameBibliotecaServices>>();
            _service = new UsuarioGameBibliotecaServices(_repositoryMock.Object, _bibliotecaRepositoryMock.Object, _gameRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ListarPorUsuario_QuandoNaoHouverItens_DeveRetornarListaVazia()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            _bibliotecaRepositoryMock.Setup(r => r.ListarPorUsuario(usuarioId)).Returns(new List<UsuarioGameBiblioteca>());

            // Act
            var result = _service.ListarPorUsuario(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ListarPorUsuario_QuandoHouverUmItem_DeveRetornarListaComUmIndice()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var biblioteca = new UsuarioGameBiblioteca
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                GameId = gameId,
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            _bibliotecaRepositoryMock.Setup(r => r.ListarPorUsuario(usuarioId)).Returns(new List<UsuarioGameBiblioteca> { biblioteca });

            // Act
            var result = _service.ListarPorUsuario(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ListarPorUsuario_QuandoHouverDoisItens_DeveRetornarListaComDoisIndices()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var gameId1 = Guid.NewGuid();
            var gameId2 = Guid.NewGuid();
            var bibliotecas = new List<UsuarioGameBiblioteca>
            {
                new UsuarioGameBiblioteca
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    GameId = gameId1,
                    TipoAquisicao = "Compra",
                    PrecoAquisicao = 99.99m,
                    DataAquisicao = DateTimeOffset.UtcNow
                },
                new UsuarioGameBiblioteca
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    GameId = gameId2,
                    TipoAquisicao = "Presente",
                    PrecoAquisicao = 0m,
                    DataAquisicao = DateTimeOffset.UtcNow
                }
            };
            _bibliotecaRepositoryMock.Setup(r => r.ListarPorUsuario(usuarioId)).Returns(bibliotecas);

            // Act
            var result = _service.ListarPorUsuario(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ComprarGame_QuandoGameNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var biblioteca = new UsuarioGameBiblioteca
            {
                UsuarioId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m
            };
            _gameRepositoryMock.Setup(r => r.GetById(biblioteca.GameId)).Returns((Game)null);

            // Act
            var result = await _service.ComprarGame(biblioteca);

            // Assert
            Assert.Null(result.Biblioteca);
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public async Task ComprarGame_QuandoUsuarioJaPossuiGame_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = Game.Criar("Jogo Teste", "Descrição", "Ação", "Dev", 99.99m, DateTimeOffset.UtcNow);
            game.Id = gameId;
            var biblioteca = new UsuarioGameBiblioteca
            {
                UsuarioId = Guid.NewGuid(),
                GameId = gameId,
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m
            };
            _gameRepositoryMock.Setup(r => r.GetById(gameId)).Returns(game);
            _bibliotecaRepositoryMock.Setup(r => r.UsuarioJaPossuiGame(biblioteca.UsuarioId, gameId)).Returns(true);

            // Act
            var result = await _service.ComprarGame(biblioteca);

            // Assert
            Assert.Null(result.Biblioteca);
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public async Task ComprarGame_QuandoCompraForBemSucedida_DeveRetornarBibliotecaComSuccessTrue()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = Game.Criar("Jogo Teste", "Descrição", "Ação", "Dev", 99.99m, DateTimeOffset.UtcNow);
            game.Id = gameId;
            var biblioteca = new UsuarioGameBiblioteca
            {
                UsuarioId = Guid.NewGuid(),
                GameId = gameId,
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m
            };
            _gameRepositoryMock.Setup(r => r.GetById(gameId)).Returns(game);
            _bibliotecaRepositoryMock.Setup(r => r.UsuarioJaPossuiGame(biblioteca.UsuarioId, gameId)).Returns(false);
            _repositoryMock.Setup(r => r.Insert(It.IsAny<UsuarioGameBiblioteca>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(biblioteca);

            // Act
            var result = await _service.ComprarGame(biblioteca);

            // Assert
            Assert.NotNull(result.Biblioteca);
            Assert.True(result.Success);
            Assert.Null(result.ErrorMessage);
            Assert.NotEqual(Guid.Empty, result.Biblioteca.Id);
        }

        [Fact]
        public async Task ComprarGame_QuandoInsercaoFalhar_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = Game.Criar("Jogo Teste", "Descrição", "Ação", "Dev", 99.99m, DateTimeOffset.UtcNow);
            game.Id = gameId;
            var biblioteca = new UsuarioGameBiblioteca
            {
                UsuarioId = Guid.NewGuid(),
                GameId = gameId,
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m
            };
            _gameRepositoryMock.Setup(r => r.GetById(gameId)).Returns(game);
            _bibliotecaRepositoryMock.Setup(r => r.UsuarioJaPossuiGame(biblioteca.UsuarioId, gameId)).Returns(false);
            _repositoryMock.Setup(r => r.Insert(It.IsAny<UsuarioGameBiblioteca>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UsuarioGameBiblioteca)null);

            // Act
            var result = await _service.ComprarGame(biblioteca);

            // Assert
            Assert.Null(result.Biblioteca);
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessage);
        }

        [Fact]
        public async Task Atualizar_QuandoBibliotecaExistir_DeveRetornarBibliotecaAtualizadaComSuccessTrue()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var bibliotecaExistente = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var bibliotecaAtualizada = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = bibliotecaExistente.GameId,
                TipoAquisicao = "Presente",
                PrecoAquisicao = 0m,
                DataAquisicao = DateTimeOffset.UtcNow.AddDays(1)
            };
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns(bibliotecaExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<UsuarioGameBiblioteca>())).Returns((bibliotecaExistente, true));

            // Act
            var result = await _service.Atualizar(bibliotecaAtualizada);

            // Assert
            Assert.NotNull(result.Biblioteca);
            Assert.True(result.Success);
            Assert.Equal(bibliotecaAtualizada.TipoAquisicao, result.Biblioteca.TipoAquisicao);
            Assert.Equal(bibliotecaAtualizada.PrecoAquisicao, result.Biblioteca.PrecoAquisicao);
            Assert.Equal(bibliotecaAtualizada.DataAquisicao, result.Biblioteca.DataAquisicao);
        }

        [Fact]
        public async Task Atualizar_QuandoBibliotecaNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var bibliotecaAtualizada = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Presente",
                PrecoAquisicao = 0m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns((UsuarioGameBiblioteca)null);

            // Act
            var result = await _service.Atualizar(bibliotecaAtualizada);

            // Assert
            Assert.Null(result.Biblioteca);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Atualizar_QuandoAtualizacaoFalhar_DeveRetornarBibliotecaComSuccessFalse()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var bibliotecaExistente = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            var bibliotecaAtualizada = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = bibliotecaExistente.GameId,
                TipoAquisicao = "Presente",
                PrecoAquisicao = 0m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns(bibliotecaExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<UsuarioGameBiblioteca>())).Returns((bibliotecaExistente, false));

            // Act
            var result = await _service.Atualizar(bibliotecaAtualizada);

            // Assert
            Assert.NotNull(result.Biblioteca);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Deletar_QuandoBibliotecaExistir_DeveRetornarTrue()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var biblioteca = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns(biblioteca);
            _repositoryMock.Setup(r => r.DeleteById(bibliotecaId)).ReturnsAsync(true);

            // Act
            var result = await _service.Deletar(bibliotecaId, usuarioId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Deletar_QuandoBibliotecaNaoExistir_DeveRetornarFalse()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns((UsuarioGameBiblioteca)null);

            // Act
            var result = await _service.Deletar(bibliotecaId, usuarioId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Deletar_QuandoDelecaoFalhar_DeveRetornarFalse()
        {
            // Arrange
            var bibliotecaId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var biblioteca = new UsuarioGameBiblioteca
            {
                Id = bibliotecaId,
                UsuarioId = usuarioId,
                GameId = Guid.NewGuid(),
                TipoAquisicao = "Compra",
                PrecoAquisicao = 99.99m,
                DataAquisicao = DateTimeOffset.UtcNow
            };
            _bibliotecaRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(bibliotecaId, usuarioId)).Returns(biblioteca);
            _repositoryMock.Setup(r => r.DeleteById(bibliotecaId)).ReturnsAsync(false);

            // Act
            var result = await _service.Deletar(bibliotecaId, usuarioId);

            // Assert
            Assert.False(result);
        }
    }
}
