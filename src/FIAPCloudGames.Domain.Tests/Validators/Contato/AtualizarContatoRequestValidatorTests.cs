using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Validators.Contato;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AtualizarContatoRequestValidatorTests
    {
        private readonly AtualizarContatoRequestValidator _validator;

        public AtualizarContatoRequestValidatorTests()
        {
            _validator = new AtualizarContatoRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_Id_estiver_vazio()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.Empty,
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = "teste@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.Id) &&
                e.ErrorMessage == "Id é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_estiver_vazio()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.Empty,
                Celular = "(12) 3456-7890",
                Email = "teste@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.UsuarioId) &&
                e.ErrorMessage == "UsuarioId é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Celular_estiver_vazio()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Celular = string.Empty,
                Email = "teste@dominio.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.Celular) &&
                e.ErrorMessage == "Celular é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_estiver_vazio()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = string.Empty
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.Email) &&
                e.ErrorMessage == "Email é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_for_invalido()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = "email-invalido"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.Email) &&
                e.ErrorMessage == "Email inválido.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Email_tiver_mais_de_100_caracteres()
        {
            var longLocal = new string('a', 95);
            var longEmail = $"{longLocal}@dominio.com";
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = longEmail
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(request.Email) &&
                e.ErrorMessage == "Email deve ter no máximo 100 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new AtualizarContatoRequest
            {
                Id = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                Celular = "(12) 3456-7890",
                Email = "valid.email@domain.com"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
