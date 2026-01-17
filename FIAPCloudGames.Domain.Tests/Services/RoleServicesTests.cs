using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests.Services
{
    public class RoleServicesTests
    {
        private readonly Mock<IGenericEntityRepository<Role>> _repositoryMock;
        private readonly Mock<ILogger<RoleServices>> _loggerMock;
        private readonly RoleServices _service;

        public RoleServicesTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Role>>();
            _loggerMock = new Mock<ILogger<RoleServices>>();
            _service = new RoleServices(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ListarRoles_QuandoNaoHouverRoles_DeveRetornarListaVazia()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Role>().AsQueryable());

            // Act
            var result = _service.ListarRoles();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ListarRoles_QuandoHouverUmaRole_DeveRetornarListaComUmIndice()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Role>
            {
                new Role { Id = 1, RoleName = "usuario", Description = "Usuário padrão" }
            }.AsQueryable());

            // Act
            var result = _service.ListarRoles();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void ListarRoles_QuandoHouverDuasRoles_DeveRetornarListaComDoisIndices()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Role>
            {
                new Role { Id = 1, RoleName = "usuario", Description = "Usuário padrão" },
                new Role { Id = 2, RoleName = "administrador", Description = "Administrador" }
            }.AsQueryable());

            // Act
            var result = _service.ListarRoles();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task AtualizarRole_QuandoRoleExistir_DeveRetornarRoleAtualizadaComSuccessTrue()
        {
            // Arrange
            var roleExistente = new Role { Id = 1, RoleName = "usuario", Description = "Descrição antiga" };
            var roleAtualizada = new Role { Id = 1, RoleName = "usuario", Description = "Descrição nova" };
            _repositoryMock.Setup(r => r.GetByIdInt(1)).Returns(roleExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Role>())).Returns((roleExistente, true));

            // Act
            var result = await _service.AtualizarRole(roleAtualizada);

            // Assert
            Assert.NotNull(result.Role);
            Assert.True(result.Success);
            Assert.Equal(roleAtualizada.RoleName, result.Role.RoleName);
            Assert.Equal(roleAtualizada.Description, result.Role.Description);
        }

        [Fact]
        public async Task AtualizarRole_QuandoRoleNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var roleAtualizada = new Role { Id = 999, RoleName = "usuario", Description = "Descrição nova" };
            _repositoryMock.Setup(r => r.GetByIdInt(999)).Returns((Role)null);

            // Act
            var result = await _service.AtualizarRole(roleAtualizada);

            // Assert
            Assert.Null(result.Role);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task AtualizarRole_QuandoAtualizacaoFalhar_DeveRetornarRoleComSuccessFalse()
        {
            // Arrange
            var roleExistente = new Role { Id = 1, RoleName = "usuario", Description = "Descrição antiga" };
            var roleAtualizada = new Role { Id = 1, RoleName = "usuario", Description = "Descrição nova" };
            _repositoryMock.Setup(r => r.GetByIdInt(1)).Returns(roleExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Role>())).Returns((roleExistente, false));

            // Act
            var result = await _service.AtualizarRole(roleAtualizada);

            // Assert
            Assert.NotNull(result.Role);
            Assert.False(result.Success);
        }
    }
}
