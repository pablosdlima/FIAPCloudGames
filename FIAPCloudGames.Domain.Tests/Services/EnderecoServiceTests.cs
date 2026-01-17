using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAPCloudGames.Domain.Tests.Services
{
    public class EnderecoServiceTests
    {
        private readonly Mock<IGenericEntityRepository<Endereco>> _repositoryMock;
        //TODO: Entender por que o contrutor de EnderecoService pede este campo
        private readonly Mock<ILogger<EnderecoService>> _loggerMock;
        private readonly Mock<IEnderecoRepository> _enderecoRepositoryMock;
        private readonly EnderecoService _service;

        public EnderecoServiceTests()
        {
            _repositoryMock = new Mock<IGenericEntityRepository<Endereco>>();
            _loggerMock = new Mock<ILogger<EnderecoService>>();
            _enderecoRepositoryMock = new Mock<IEnderecoRepository>();
            _service = new EnderecoService(_repositoryMock.Object, _enderecoRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void ListarPorUsusario_QuandoNaoHouverUsuario_DeveRetornarListaVazia()
        {
            // Arrange
            var id = Guid.NewGuid();
            _enderecoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Endereco>());

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ListarPorUsusario_QuandoHouverUmUsuario_DeveRetornarListaComUmIndice()
        {
            // Arrange
            var id = Guid.NewGuid();
            _enderecoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Endereco> 
            { 
                new Endereco 
                { 
                    Rua = "Rua Teste", 
                    Numero = "123", 
                    Bairro = "Centro", 
                    Cidade = "São Paulo", 
                    Estado = "SP", 
                    Cep = "01234567" 
                } 
            });

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
            _enderecoRepositoryMock.Setup(e => e.ListarPorUsuario(id)).Returns(new List<Endereco> 
            { 
                new Endereco 
                { 
                    Rua = "Rua Teste 1", 
                    Numero = "123", 
                    Bairro = "Centro", 
                    Cidade = "São Paulo", 
                    Estado = "SP", 
                    Cep = "01234567" 
                },
                new Endereco 
                { 
                    Rua = "Rua Teste 2", 
                    Numero = "456", 
                    Bairro = "Jardim", 
                    Cidade = "Rio de Janeiro", 
                    Estado = "RJ", 
                    Cep = "12345678" 
                }
            });

            // Act
            var result = _service.ListarPorUsuario(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroForBemSucedido_DeveRetornarEnderecoComIdAtribuido()
        {
            // Arrange
            var endereco = new Endereco
            {
                Rua = "Rua Teste",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Endereco>(), It.IsAny<CancellationToken>())).ReturnsAsync(endereco);

            // Act
            var result = await _service.Cadastrar(endereco);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(endereco.Rua, result.Rua);
            Assert.Equal(endereco.Numero, result.Numero);
            Assert.Equal(endereco.Bairro, result.Bairro);
            Assert.Equal(endereco.Cidade, result.Cidade);
            Assert.Equal(endereco.Estado, result.Estado);
            Assert.Equal(endereco.Cep, result.Cep);
        }

        [Fact]
        public async Task Cadastrar_QuandoCadastroFalhar_DeveRetornarNull()
        {
            // Arrange
            var endereco = new Endereco
            {
                Rua = "Rua Teste",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Endereco>(), It.IsAny<CancellationToken>())).ReturnsAsync((Endereco)null);

            // Act
            var result = await _service.Cadastrar(endereco);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Atualizar_QuandoEnderecoExistir_DeveRetornarEnderecoAtualizadoComSuccessTrue()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var enderecoExistente = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Antiga",
                Numero = "123",
                Complemento = "Apto 1",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            var enderecoAtualizado = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Nova",
                Numero = "456",
                Complemento = "Apto 2",
                Bairro = "Jardim",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Cep = "12345678"
            };
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns(enderecoExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Endereco>())).Returns((enderecoExistente, true));

            // Act
            var result = await _service.Atualizar(enderecoAtualizado);

            // Assert
            Assert.NotNull(result.Endereco);
            Assert.True(result.Success);
            Assert.Equal(enderecoAtualizado.Rua, result.Endereco.Rua);
            Assert.Equal(enderecoAtualizado.Numero, result.Endereco.Numero);
            Assert.Equal(enderecoAtualizado.Complemento, result.Endereco.Complemento);
            Assert.Equal(enderecoAtualizado.Bairro, result.Endereco.Bairro);
            Assert.Equal(enderecoAtualizado.Cidade, result.Endereco.Cidade);
            Assert.Equal(enderecoAtualizado.Estado, result.Endereco.Estado);
            Assert.Equal(enderecoAtualizado.Cep, result.Endereco.Cep);
        }

        [Fact]
        public async Task Atualizar_QuandoEnderecoNaoExistir_DeveRetornarNullComSuccessFalse()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var enderecoAtualizado = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Nova",
                Numero = "456",
                Bairro = "Jardim",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Cep = "12345678"
            };
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns((Endereco)null);

            // Act
            var result = await _service.Atualizar(enderecoAtualizado);

            // Assert
            Assert.Null(result.Endereco);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Atualizar_QuandoAtualizacaoFalhar_DeveRetornarEnderecoComSuccessFalse()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var enderecoExistente = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Antiga",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            var enderecoAtualizado = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Nova",
                Numero = "456",
                Bairro = "Jardim",
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Cep = "12345678"
            };
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns(enderecoExistente);
            _repositoryMock.Setup(r => r.Update(It.IsAny<Endereco>())).Returns((enderecoExistente, false));

            // Act
            var result = await _service.Atualizar(enderecoAtualizado);

            // Assert
            Assert.NotNull(result.Endereco);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Deletar_QuandoEnderecoExistir_DeveRetornarTrue()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var endereco = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Teste",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns(endereco);
            _repositoryMock.Setup(r => r.DeleteById(enderecoId)).ReturnsAsync(true);

            // Act
            var result = await _service.Deletar(enderecoId, usuarioId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Deletar_QuandoEnderecoNaoExistir_DeveRetornarFalse()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns((Endereco)null);

            // Act
            var result = await _service.Deletar(enderecoId, usuarioId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Deletar_QuandoDelecaoFalhar_DeveRetornarFalse()
        {
            // Arrange
            var enderecoId = Guid.NewGuid();
            var usuarioId = Guid.NewGuid();
            var endereco = new Endereco
            {
                Id = enderecoId,
                UsuarioId = usuarioId,
                Rua = "Rua Teste",
                Numero = "123",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01234567"
            };
            _enderecoRepositoryMock.Setup(r => r.BuscarPorIdEUsuario(enderecoId, usuarioId)).Returns(endereco);
            _repositoryMock.Setup(r => r.DeleteById(enderecoId)).ReturnsAsync(false);

            // Act
            var result = await _service.Deletar(enderecoId, usuarioId);

            // Assert
            Assert.False(result);
        }
    }
}
