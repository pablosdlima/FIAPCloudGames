using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Validators.Enderecos;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AtualizarEnderecoRequestValidatorTests
    {
        private readonly AtualizarEnderecoRequestValidator _validator;

        public AtualizarEnderecoRequestValidatorTests()
        {
            _validator = new AtualizarEnderecoRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_Id_estiver_vazio()
        {
            var request = new AtualizarEnderecoRequest
            {
                Id = Guid.Empty,
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Id) && e.ErrorMessage == "Id é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.Empty,
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "UsuarioId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Rua_estiver_vazia_ou_maior_que_200_caracteres()
        {
            var tooLong = new string('a', 201);
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = string.Empty,
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Rua) && e.ErrorMessage == "Rua é obrigatória.");

            var requestLong = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = tooLong,
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Rua) && e.ErrorMessage == "Rua deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Numero_estiver_vazio_ou_maior_que_20_caracteres()
        {
            var tooLong = new string('1', 21);
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = string.Empty,
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Numero) && e.ErrorMessage == "Número é obrigatório.");

            var requestLong = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = tooLong,
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Numero) && e.ErrorMessage == "Número deve ter no máximo 20 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Complemento_maior_que_100_caracteres()
        {
            var tooLong = new string('c', 101);
            var request = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = tooLong,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Complemento) && e.ErrorMessage == "Complemento deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Bairro_estiver_vazio_ou_maior_que_100_caracteres()
        {
            var tooLong = new string('b', 101);
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = string.Empty,
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Bairro) && e.ErrorMessage == "Bairro é obrigatório.");

            var requestLong = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = tooLong,
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Bairro) && e.ErrorMessage == "Bairro deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cidade_estiver_vazia_ou_maior_que_100_caracteres()
        {
            var tooLong = new string('c', 101);
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = string.Empty,
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Cidade) && e.ErrorMessage == "Cidade é obrigatória.");

            var requestLong = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = tooLong,
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultLong = _validator.Validate(requestLong);

            resultLong.IsValid.Should().BeFalse();
            resultLong.Errors.Should().Contain(e => e.PropertyName == nameof(requestLong.Cidade) && e.ErrorMessage == "Cidade deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Estado_estiver_vazio_ou_nao_tiver_2_caracteres()
        {
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = string.Empty,
                Cep = "12345-678"
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Estado) && e.ErrorMessage == "Estado é obrigatório.");

            var requestInvalidLength = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SPBR",
                Cep = "12345-678"
            };

            var resultInvalidLength = _validator.Validate(requestInvalidLength);

            resultInvalidLength.IsValid.Should().BeFalse();
            resultInvalidLength.Errors.Should().Contain(e => e.PropertyName == nameof(requestInvalidLength.Estado) && e.ErrorMessage == "Estado deve ter 2 caracteres (UF).");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cep_estiver_vazio_ou_formato_invalido()
        {
            var requestEmpty = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = string.Empty
            };

            var resultEmpty = _validator.Validate(requestEmpty);

            resultEmpty.IsValid.Should().BeFalse();
            resultEmpty.Errors.Should().Contain(e => e.PropertyName == nameof(requestEmpty.Cep) && e.ErrorMessage == "CEP é obrigatório.");

            var requestInvalid = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "1234"
            };

            var resultInvalid = _validator.Validate(requestInvalid);

            resultInvalid.IsValid.Should().BeFalse();
            resultInvalid.Errors.Should().Contain(e => e.PropertyName == nameof(requestInvalid.Cep) && e.ErrorMessage == "CEP deve estar no formato 00000-000 ou 00000000.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new AtualizarEnderecoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua A",
                Numero = "10",
                Complemento = "Apto 1",
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
