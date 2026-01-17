using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests.Services
{
    public class UsuarioPerfilServicesTests
    {
        private readonly Mock<IGenericEntityRepository<UsuarioPerfil>> _repositoryMock;
        private readonly Mock<IUsuarioPerfilRepository> _usuarioPerfilRepositoryMock;
        private readonly Mock<ILogger<UsuarioPerfilServices>> _loggerMock;
        private readonly UsuarioPerfilServices _service;

        public UsuarioPerfilServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<UsuarioPerfil>>();
            _usuarioPerfilRepositoryMock = new Mock<IUsuarioPerfilRepository>();
            _loggerMock = new Mock<ILogger<UsuarioPerfilServices>>();
            _service = new UsuarioPerfilServices(_repositoryMock.Object, _usuarioPerfilRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void BuscarPorUsuarioId_QuandoNaoHouverPerfil_DeveRetornarNull()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorUsuarioId(usuarioId)).Returns((UsuarioPerfil)null);

            // Act
            var result = _service.BuscarPorUsuarioId(usuarioId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BuscarPorUsuarioId_QuandoHouverPerfil_DeveRetornarPerfil()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var perfil = new UsuarioPerfil("Nome Completo", DateTimeOffset.UtcNow, "Brasil", "avatar.jpg")
            {
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorUsuarioId(usuarioId)).Returns(perfil);

            // Act
            var result = _service.BuscarPorUsuarioId(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(perfil.NomeCompleto, result.NomeCompleto);
            Assert.Equal(perfil.UsuarioId, result.UsuarioId);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroForBemSucedido_DeveRetornarPerfil()
        {
            // Arrange
            var perfil = new UsuarioPerfil("Nome Completo", DateTimeOffset.UtcNow, "Brasil", "avatar.jpg");
            _repositoryMock.Setup(r => r.Insert(It.IsAny<UsuarioPerfil>(), It.IsAny<CancellationToken>())).ReturnsAsync(perfil);

            // Act
            var result = await _service.Cadastrar(perfil);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(perfil.NomeCompleto, result.NomeCompleto);
            Assert.Equal(perfil.Pais, result.Pais);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroFalhar_DeveRetornarNull()
        {
            // Arrange
            var perfil = new UsuarioPerfil("Nome Completo", DateTimeOffset.UtcNow, "Brasil", "avatar.jpg");
            _repositoryMock.Setup(r => r.Insert(It.IsAny<UsuarioPerfil>(), It.IsAny<CancellationToken>())).ReturnsAsync((UsuarioPerfil)null);

            // Act
            var result = await _service.Cadastrar(perfil);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Atualizar_QuandoPerfilExistir_DeveRetornarPerfilAtualizadoComSuccessTrue()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var perfilExistente = new UsuarioPerfil("Nome Antigo", DateTimeOffset.UtcNow, "Brasil", "avatar-antigo.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            var perfilAtualizado = new UsuarioPerfil("Nome Novo", DateTimeOffset.UtcNow.AddYears(-1), "EUA", "avatar-novo.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns(perfilExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<UsuarioPerfil>())).Returns((perfilExistente, true));

            // Act
            var result = await _service.Atualizar(perfilAtualizado);

            // Assert
            Assert.NotNull(result.Perfil);
            Assert.True(result.Success);
            Assert.Equal(perfilAtualizado.NomeCompleto, result.Perfil.NomeCompleto);
            Assert.Equal(perfilAtualizado.DataNascimento, result.Perfil.DataNascimento);
            Assert.Equal(perfilAtualizado.Pais, result.Perfil.Pais);
            Assert.Equal(perfilAtualizado.AvatarUrl, result.Perfil.AvatarUrl);
        }

        [Fact]
        public async Task Atualizar_QuandoPerfilNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var perfilAtualizado = new UsuarioPerfil("Nome Novo", DateTimeOffset.UtcNow, "EUA", "avatar-novo.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns((UsuarioPerfil)null);

            // Act
            var result = await _service.Atualizar(perfilAtualizado);

            // Assert
            Assert.Null(result.Perfil);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Atualizar_QuandoAtualizacaoFalhar_DeveRetornarPerfilComSuccessFalse()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var perfilExistente = new UsuarioPerfil("Nome Antigo", DateTimeOffset.UtcNow, "Brasil", "avatar-antigo.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            var perfilAtualizado = new UsuarioPerfil("Nome Novo", DateTimeOffset.UtcNow, "EUA", "avatar-novo.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns(perfilExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<UsuarioPerfil>())).Returns((perfilExistente, false));

            // Act
            var result = await _service.Atualizar(perfilAtualizado);

            // Assert
            Assert.NotNull(result.Perfil);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Deletar_QuandoPerfilExistir_DeveRetornarTrue()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var perfil = new UsuarioPerfil("Nome Completo", DateTimeOffset.UtcNow, "Brasil", "avatar.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns(perfil);
            _repositoryMock.Setup(r => r.DeleteById(perfilId)).ReturnsAsync(true);

            // Act
            var result = await _service.Deletar(perfilId, usuarioId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Deletar_QuandoPerfilNaoExistir_DeveRetornarFalse()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns((UsuarioPerfil)null);

            // Act
            var result = await _service.Deletar(perfilId, usuarioId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Deletar_QuandoDelecaoFalhar_DeveRetornarFalse()
        {
            // Arrange
            var perfilId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var perfil = new UsuarioPerfil("Nome Completo", DateTimeOffset.UtcNow, "Brasil", "avatar.jpg")
            {
                Id = perfilId,
                UsuarioId = usuarioId
            };
            _usuarioPerfilRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(perfilId, usuarioId)).Returns(perfil);
            _repositoryMock.Setup(r => r.DeleteById(perfilId)).ReturnsAsync(false);

            // Act
            var result = await _service.Deletar(perfilId, usuarioId);

            // Assert
            Assert.False(result);
        }
    }
}
