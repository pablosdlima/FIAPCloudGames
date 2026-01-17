using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests
{
    public class UsuarioServicesTests
    {
        private readonly Mock<IGenericEntityRepository<Usuario>> _repositoryMock;
        private readonly Mock<ILogger<UsuarioServices>> _loggerMock;
        private readonly UsuarioServices _service;

        public UsuarioServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Usuario>>();
            _loggerMock = new Mock<ILogger<UsuarioServices>>();
            _service = new UsuarioServices(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CadastrarUsuario_QuandoCadastroForBemSucedido_DeveRetornarUsuario()
        {
            // Arrange
            var request = new CadastrarUsuarioRequest
            {
                Nome = "usuario_teste",
                Senha = "senha123",
                Celular = "11999999999",
                Email = "teste@email.com",
                TipoUsuario = TipoUsuario.Usuario,
                NomeCompleto = "Nome Completo",
                DataNascimento = DateTimeOffset.UtcNow,
                Pais = "Brasil",
                AvatarUrl = "avatar.jpg"
            };
            var usuarioExistente = Usuario.Criar("usuario_existente", "senha_hash");
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario>().AsQueryable());
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioExistente);

            // Act
            var result = await _service.CadastrarUsuario(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Nome, result.Nome);
            Assert.NotNull(result.Contatos);
            Assert.Single(result.Contatos);
            Assert.NotNull(result.Perfil);
        }

        [Fact]
        public async Task CadastrarUsuario_QuandoNomeJaExistir_DeveLancarDomainException()
        {
            // Arrange
            var request = new CadastrarUsuarioRequest
            {
                Nome = "usuario_existente",
                Senha = "senha123",
                Celular = "11999999999",
                Email = "teste@email.com",
                TipoUsuario = TipoUsuario.Usuario,
                NomeCompleto = "Nome Completo",
                DataNascimento = DateTimeOffset.UtcNow,
                Pais = "Brasil",
                AvatarUrl = "avatar.jpg"
            };
            var usuarioExistente = Usuario.Criar("usuario_existente", "senha_hash");
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuarioExistente }.AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() => _service.CadastrarUsuario(request));
        }

        [Fact]
        public async Task ValidarLogin_QuandoCredenciaisForemValidas_DeveRetornarUsuario()
        {
            // Arrange
            var nome = "usuario_teste";
            var senha = "senha123";
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
            var usuario = Usuario.Criar(nome, senhaHash, true);
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act
            var result = await _service.ValidarLogin(nome, senha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nome, result.Nome);
        }

        [Fact]
        public async Task ValidarLogin_QuandoUsuarioNaoExistir_DeveLancarAutenticacaoException()
        {
            // Arrange
            var nome = "usuario_inexistente";
            var senha = "senha123";
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<AutenticacaoException>(() => _service.ValidarLogin(nome, senha));
        }

        [Fact]
        public async Task ValidarLogin_QuandoSenhaForIncorreta_DeveLancarAutenticacaoException()
        {
            // Arrange
            var nome = "usuario_teste";
            var senhaCorreta = "senha123";
            var senhaIncorreta = "senha_errada";
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senhaCorreta);
            var usuario = Usuario.Criar(nome, senhaHash, true);
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<AutenticacaoException>(() => _service.ValidarLogin(nome, senhaIncorreta));
        }

        [Fact]
        public async Task ValidarLogin_QuandoUsuarioEstiverInativo_DeveLancarAutenticacaoException()
        {
            // Arrange
            var nome = "usuario_teste";
            var senha = "senha123";
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
            var usuario = Usuario.Criar(nome, senhaHash, false);
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<AutenticacaoException>(() => _service.ValidarLogin(nome, senha));
        }

        [Fact]
        public async Task AlterarSenha_QuandoAlteracaoForBemSucedida_DeveRetornarTrue()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new AlterarSenhaRequest(usuarioId, "nova_senha");
            var usuario = Usuario.Criar("usuario_teste", "senha_antiga_hash", true);
            usuario.Id = usuarioId;
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuario }.AsQueryable());
            _repositoryMock.Setup(r => r.Update(It.IsAny<Usuario>())).Returns((usuario, true));

            // Act
            var result = await _service.AlterarSenha(request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AlterarSenha_QuandoUsuarioNaoExistir_DeveLancarNotFoundException()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var request = new AlterarSenhaRequest(usuarioId, "nova_senha");
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.AlterarSenha(request));
        }

        [Fact]
        public async Task AlterarStatus_QuandoAlteracaoForBemSucedida_DeveRetornarResponse()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var usuario = Usuario.Criar("usuario_teste", "senha_hash", true);
            usuario.Id = usuarioId;
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario> { usuario }.AsQueryable());
            _repositoryMock.Setup(r => r.Update(It.IsAny<Usuario>())).Returns((usuario, true));

            // Act
            var result = await _service.AlterarStatus(usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Inativo", result.StatusAtual);
        }

        [Fact]
        public async Task AlterarStatus_QuandoUsuarioNaoExistir_DeveLancarNotFoundException()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Usuario>().AsQueryable());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.AlterarStatus(usuarioId));
        }

        [Fact]
        public async Task BuscarPorIdsAsync_QuandoHouverUsuarios_DeveRetornarDicionario()
        {
            // Arrange
            var usuarioId1 = Guid.NewGuid();
            var usuarioId2 = Guid.NewGuid();
            var usuarios = new List<Usuario>
            {
                new Usuario() { Id = usuarioId1 },
                new Usuario() { Id = usuarioId2 },
            };
            var ids = new[] { usuarioId1, usuarioId2 };
            _repositoryMock.Setup(r => r.BuscarPorIdsAsync(ids, It.IsAny<System.Linq.Expressions.Expression<Func<Usuario, Guid>>>()))
                .ReturnsAsync(usuarios);

            // Act
            var result = await _service.BuscarPorIdsAsync(ids);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(usuarioId1, result.Keys);
            Assert.Contains(usuarioId2, result.Keys);
        }
    }
}
