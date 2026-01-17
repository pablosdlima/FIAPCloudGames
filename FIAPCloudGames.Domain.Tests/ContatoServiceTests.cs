using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests
{
    public class ContatoServiceTests
    {
        private readonly Mock<IGenericEntityRepository<Contato>> _repositoryMock;
        //TODO: Entender por que o contrutor de ContatoService pede este campo
        private readonly Mock<ILogger<ContatoService>> _loggerMock;
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly ContatoService _service;

        public ContatoServiceTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Contato>>();
            _loggerMock = new Mock<ILogger<ContatoService>>();
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _service = new ContatoService(_repositoryMock.Object, _contatoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ListarPorUsusario_QuandoNaoHouverUsuario_DeveRetornarListaVazia()
        {
            // Arrange
            var id = Guid.NewGuid();
            _contatoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Contato>());

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ListarPorUsusario_QuandoNaoHouverUmUsuario_DeveRetornarListaComUmIndice()
        {
            // Arrange
            var id = Guid.NewGuid();
            _contatoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Contato> { new Contato("phone", "email") });

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ListarPorUsusario_QuandoHouverDoisUsuarios_DeveRetornarListaComDoisIndices()
        {
            // Arrange
            var id = Guid.NewGuid();
            _contatoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Contato> { new Contato("phone1", "email1"), new Contato("phone2", "email2") });

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroForBemSucedido_DeveRetornarContato()
        {
            // Arrange
            var contato = new Contato("11999999999", "teste@email.com");
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Contato>(), It.IsAny<CancellationToken>())).ReturnsAsync(contato);

            // Act
            var result = await _service.Cadastrar(contato);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contato.Celular, result.Celular);
            Assert.Equal(contato.Email, result.Email);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroFalhar_DeveRetornarNull()
        {
            // Arrange
            var contato = new Contato("11999999999", "teste@email.com");
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Contato>(), It.IsAny<CancellationToken>())).ReturnsAsync((Contato)null);

            // Act
            var result = await _service.Cadastrar(contato);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Atualizar_QuandoContatoExistir_DeveRetornarContatoAtualizadoComSuccessTrue()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var contatoExistente = new Contato("11999999999", "antigo@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            var contatoAtualizado = new Contato("11888888888", "novo@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns(contatoExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Contato>())).Returns((contatoExistente, true));

            // Act
            var result = await _service.Atualizar(contatoAtualizado);

            // Assert
            Assert.NotNull(result.Contato);
            Assert.True(result.Success);
            Assert.Equal(contatoAtualizado.Celular, result.Contato.Celular);
            Assert.Equal(contatoAtualizado.Email, result.Contato.Email);
        }

        [Fact]
        public async Task Atualizar_QuandoContatoNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var contatoAtualizado = new Contato("11888888888", "novo@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns((Contato)null);

            // Act
            var result = await _service.Atualizar(contatoAtualizado);

            // Assert
            Assert.Null(result.Contato);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Atualizar_QuandoAtualizacaoFalhar_DeveRetornarContatoComSuccessFalse()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var contatoExistente = new Contato("11999999999", "antigo@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            var contatoAtualizado = new Contato("11888888888", "novo@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns(contatoExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Contato>())).Returns((contatoExistente, false));

            // Act
            var result = await _service.Atualizar(contatoAtualizado);

            // Assert
            Assert.NotNull(result.Contato);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Deletar_QuandoContatoExistir_DeveRetornarTrue()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var contato = new Contato("11999999999", "teste@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns(contato);
            _repositoryMock.Setup(r => r.DeleteById(contatoId)).ReturnsAsync(true);

            // Act
            var result = await _service.Deletar(contatoId, usuarioId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Deletar_QuandoContatoNaoExistir_DeveRetornarFalse()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns((Contato)null);

            // Act
            var result = await _service.Deletar(contatoId, usuarioId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Deletar_QuandoDelecaoFalhar_DeveRetornarFalse()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var contato = new Contato("11999999999", "teste@email.com")
            {
                Id = contatoId,
                UsuarioId = usuarioId
            };
            _contatoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(contatoId, usuarioId)).Returns(contato);
            _repositoryMock.Setup(r => r.DeleteById(contatoId)).ReturnsAsync(false);

            // Act
            var result = await _service.Deletar(contatoId, usuarioId);

            // Assert
            Assert.False(result);
        }
    }
}
