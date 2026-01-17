using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Validators.Enderecos;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class CadastrarEnderecoRequestValidatorTests
    {
        private readonly CadastrarEnderecoRequestValidator _validator;

        public CadastrarEnderecoRequestValidatorTests()
        {
            _validator = new CadastrarEnderecoRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.Empty,
                Rua = "Rua X",
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
        public void Deve_ter_erro_quando_Rua_estiver_vazia()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = string.Empty,
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Rua) && e.ErrorMessage == "Rua é obrigatória.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Rua_tiver_mais_de_200_caracteres()
        {
            var longRua = new string('a', 201);
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = longRua,
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Rua) && e.ErrorMessage == "Rua deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Numero_estiver_vazio()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = string.Empty,
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Numero) && e.ErrorMessage == "Número é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Numero_tiver_mais_de_20_caracteres()
        {
            var longNumero = new string('1', 21);
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = longNumero,
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Numero) && e.ErrorMessage == "Número deve ter no máximo 20 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Complemento_tiver_mais_de_100_caracteres()
        {
            var longComplemento = new string('c', 101);
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = longComplemento,
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
        public void Deve_ter_erro_quando_Bairro_estiver_vazio()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = string.Empty,
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Bairro) && e.ErrorMessage == "Bairro é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Bairro_tiver_mais_de_100_caracteres()
        {
            var longBairro = new string('b', 101);
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = longBairro,
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Bairro) && e.ErrorMessage == "Bairro deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cidade_estiver_vazia()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = string.Empty,
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Cidade) && e.ErrorMessage == "Cidade é obrigatória.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cidade_tiver_mais_de_100_caracteres()
        {
            var longCidade = new string('c', 101);
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = longCidade,
                Estado = "SP",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Cidade) && e.ErrorMessage == "Cidade deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Estado_estiver_vazio()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = string.Empty,
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Estado) && e.ErrorMessage == "Estado é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Estado_nao_tiver_2_caracteres()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "S",
                Cep = "12345-678"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Estado) && e.ErrorMessage == "Estado deve ter 2 caracteres (UF).");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cep_estiver_vazio()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = string.Empty
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Cep) && e.ErrorMessage == "CEP é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Cep_tiver_formato_invalido()
        {
            var request = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "1234"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Cep) && e.ErrorMessage == "CEP deve estar no formato 00000-000 ou 00000000.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos_e_complemento_nulo_ou_presente()
        {
            var requestA = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = null,
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345-678"
            };

            var resultA = _validator.Validate(requestA);

            resultA.IsValid.Should().BeTrue();
            resultA.Errors.Should().BeEmpty();

            var requestB = new CadastrarEnderecoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Rua = "Rua X",
                Numero = "10",
                Complemento = "Apto 1",
                Bairro = "Bairro",
                Cidade = "Cidade",
                Estado = "SP",
                Cep = "12345678"
            };

            var resultB = _validator.Validate(requestB);

            resultB.IsValid.Should().BeTrue();
            resultB.Errors.Should().BeEmpty();
        }
    }
}
