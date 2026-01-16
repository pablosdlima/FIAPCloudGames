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
            _repositoryMock.Setup(e => e.Get()).Returns(new List<Contato>().AsQueryable());
            var id = Guid.NewGuid();

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
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Contato> { new Contato("phone", "email") }.AsQueryable());
            var id = Guid.NewGuid();


            // Act
            var result = _service.ListarPorUsuario(id);

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
            _repositoryMock.Setup(r => r.Get()).Returns(new List<Contato> {new Contato("phone1", "email1"), new Contato("phone2", "email2")}.AsQueryable());
            var id = Guid.NewGuid();

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }
    }
}
