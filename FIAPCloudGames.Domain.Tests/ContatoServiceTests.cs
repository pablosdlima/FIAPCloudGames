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
        private readonly Mock<IGenericEntityRepository<Contato>> _contatoEntityMock;
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly Mock<ILogger<ContatoService>> _contatoServiceLogger;

        public ContatoServiceTests()
        {
        }

        [Fact]
        public void ListarPorUsusario_QuandoNaoHouverUsuario_DeveRetornarListaVazia()
        {
            // Arrange
            //TODO: Entender porque contatoEntity tem esse campo!
            _contatoEntityMock.Setup(e => e.Get()).Returns(new List<Contato>().AsQueryable());
            _contatoRepositoryMock.Setup(r => r.Get()).Returns(new List<Contato>().AsQueryable());
            var contatoService = new ContatoService(_contatoEntityMock.Object, _contatoRepositoryMock.Object, _contatoServiceLogger.Object);

            // Act
            var result = contatoService.ListarPorUsuario(new Guid());

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ListarPorUsusario_QuandoNaoHouverUmUsuario_DeveRetornarListaComUmIndice()
        {
            // Arrange
            _contatoRepositoryMock.Setup(r => r.Get()).Returns(new List<Contato> { new Contato("phone", "email") }.AsQueryable());
            var contatoService = new ContatoService(_contatoEntityMock.Object, _contatoRepositoryMock.Object, _contatoServiceLogger.Object);

            // Act
            var result = contatoService.ListarPorUsuario(new Guid());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
            //para verificar se um método foi chamado, loggers por exemplo:
            //_contatoServiceLoggerMock.Verify(r => r.Log(It.IsAny<string>(), Times.Once"ou equals x", ou AtLeast);
        }

        [Fact]
        public void ListarPorUsusario_QuandoHouverDoisUsuarios_DeveRetornarListaComDoisIndices()
        {
            // Arrange
            _contatoRepositoryMock.Setup(r => r.Get()).Returns(new List<Contato>
            {
                new Contato("phone1", "email1"),
                new Contato("phone2", "email2")
            }.AsQueryable());
            var contatoService = new ContatoService(_contatoEntityMock.Object, _contatoRepositoryMock.Object, _contatoServiceLogger.Object);

            // Act
            var result = contatoService.ListarPorUsuario(new Guid());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }
    }
}
