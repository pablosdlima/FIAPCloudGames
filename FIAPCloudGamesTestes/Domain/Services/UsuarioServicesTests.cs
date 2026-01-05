using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace FIAPCloudGames.Tests.Domain.Services
{
    public class UsuarioServicesTests
    {
        private readonly Mock<IGenericEntityRepository<Usuario>> _repositoryMock;
        private readonly UsuarioServices _service;

        public UsuarioServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Usuario>>();
            _service = new UsuarioServices(_repositoryMock.Object);
        }

        [Fact]
        public async Task ValidarLogin_QuandoUsuarioNaoExistir_DeveLancarAutenticacaoException()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.Get())
                .Returns(new List<Usuario>().AsQueryable());

            // Act
            Func<Task> act = async () =>
                await _service.ValidarLogin("usuario_inexistente", "zQc$B560");

            // Assert
            await act.Should()
                .ThrowAsync<AutenticacaoException>()
                .WithMessage("Usuário não encontrado.");
        }
        [Fact]
        public async Task ValidarLogin_QuandoSenhaForIncorreta_DeveLancarAutenticacaoException()
        {
            // Arrange
            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword("zQc$B560");
            var usuario = Usuario.Criar("maria", senhaCriptografada);

            _repositoryMock
                .Setup(r => r.Get())
                .Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act
            Func<Task> act = async () =>
                await _service.ValidarLogin("maria", "SenhaErrada");

            // Assert
            await act.Should()
                .ThrowAsync<AutenticacaoException>()
                .WithMessage("Senha incorreta.");
        }
        [Fact]
        public async Task ValidarLogin_QuandoUsuarioEstiverInativo_DeveLancarAutenticacaoException()
        {
            // Arrange
            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword("zQc$B560");
            var usuario = Usuario.Criar("maria", senhaCriptografada);
            usuario.AlterarStatus(false);

            _repositoryMock
                .Setup(r => r.Get())
                .Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act
            Func<Task> act = async () =>
                await _service.ValidarLogin("maria", "zQc$B560");

            // Assert
            await act.Should()
                .ThrowAsync<AutenticacaoException>()
                .WithMessage("Usuário inativo.");
        }
        [Fact]
        public async Task ValidarLogin_QuandoCredenciaisForemValidas_DeveRetornarUsuario()
        {
            // Arrange
            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword("zQc$B560");
            var usuario = Usuario.Criar("maria", senhaCriptografada);

            _repositoryMock
                .Setup(r => r.Get())
                .Returns(new List<Usuario> { usuario }.AsQueryable());

            // Act
            var result = await _service.ValidarLogin("maria", "zQc$B560");

            // Assert
            result.Should().NotBeNull();
            result.Nome.Should().Be("maria");
        }
    }
}
