using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Validators.Usuarios;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AlterarSenhaRequestValidatorTests
    {
        private readonly AlterarSenhaRequestValidator _validator;

        public AlterarSenhaRequestValidatorTests()
        {
            _validator = new AlterarSenhaRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_Id_estiver_vazio()
        {
            var request = new AlterarSenhaRequest(Guid.Empty, "senhaValida");

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Id) && e.ErrorMessage == "O ID do usuário é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Senha_estiver_vazia()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), string.Empty);

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Senha) && e.ErrorMessage == "A senha é obrigatória.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Senha_tiver_menos_de_6_caracteres()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), "12345");

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Senha) && e.ErrorMessage == "A senha deve ter no mínimo 6 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_Id_e_Senha_forem_validos()
        {
            var request = new AlterarSenhaRequest(Guid.NewGuid(), "senha123");

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
