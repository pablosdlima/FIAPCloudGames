using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Validators.Contato;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class CadastrarContatoRequestValidatorTests
    {
        private readonly CadastrarContatoRequestValidator _validator;

        public CadastrarContatoRequestValidatorTests()
        {
            _validator = new CadastrarContatoRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.Empty,
                Celular = "(12) 3456-7890",
                Email = "teste@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "UsuarioId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Celular_estiver_vazio()
        {
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Celular = string.Empty,
                Email = "teste@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Celular) && e.ErrorMessage == "Celular é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_estiver_vazio()
        {
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = string.Empty
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == "Email é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_for_invalido()
        {
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = "nao-e-um-email"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == "Email inválido.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_for_maior_que_100_caracteres()
        {
            var longLocal = new string('a', 95);
            var longEmail = $"{longLocal}@dominio.com";
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = longEmail
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Email) && e.ErrorMessage == "Email deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new CadastrarContatoRequest
            {
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = "usuario@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
