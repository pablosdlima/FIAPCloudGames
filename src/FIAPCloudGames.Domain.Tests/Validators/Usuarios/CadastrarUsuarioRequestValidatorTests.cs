using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Validators.Usuarios;
using FIAPCloudGames.Domain.Enums;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class CadastrarUsuarioRequestValidatorTests
    {
        private readonly CadastrarUsuarioRequestValidator _validator;

        public CadastrarUsuarioRequestValidatorTests()
        {
            _validator = new CadastrarUsuarioRequestValidator();
        }

        private static CadastrarUsuarioRequest CriarRequestValitor()
        {
            return new CadastrarUsuarioRequest
            {
                Nome = "Maria Eduarda",
                Email = "maria@email.com",
                Senha = "zQc$B560",
                Celular = "11999999999",
                TipoUsuario = TipoUsuario.Administrador
            };
        }

        [Fact]
        public void Validator_QuandoEmailForValido_DeveRetornarSucesso()
        {
            //Arange
            var request = CriarRequestValitor();

            //Act
            var result = _validator.Validate(request);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_QuandoEmailForInvalido_DeveRetornarErro()
        {
            //Arange
            var request = CriarRequestValitor();
            request.Email = "email-invalido";

            //Act
            var result = _validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
        }

        [Fact]
        public void Validator_QuandoSenhaForte_DeveRetornarSucesso()
        {
            //Arange
            var request = CriarRequestValitor();

            //Act
            var result = _validator.Validate(request);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_QuandoSenhaFraca_DeveRetornarErro()
        {
            //Arange
            var request = CriarRequestValitor();
            request.Senha = "123456";

            //Act
            var result = _validator.Validate(request);

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Senha");
        }
    }
}
