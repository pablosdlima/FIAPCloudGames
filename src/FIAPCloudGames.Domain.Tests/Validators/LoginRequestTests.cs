using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Validators.Authentication;
using FluentAssertions;
using Xunit;

namespace FIAPCloudGames.Tests.Domain.Validators
{
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator;

        public LoginRequestValidatorTests()
        {
            _validator = new LoginRequestValidator();
        }

        private static LoginRequest CriarRequestValido()
        {
            return new LoginRequest
            {
                Usuario = "maria",
                Senha = "zQc$B560"
            };
        }

        [Fact]
        public void Validator_QuandoDadosLoginForemValidos_DeveRetornarSucesso()
        {
            // Arrange
            var request = CriarRequestValido();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validator_QuandoUsuarioNaoForInformado_DeveRetornarErro()
        {
            // Arrange
            var request = CriarRequestValido();
            request.Usuario = string.Empty;

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == nameof(LoginRequest.Usuario));
        }

        [Fact]
        public void Validator_QuandoSenhaNaoForInformada_DeveRetornarErro()
        {
            // Arrange
            var request = CriarRequestValido();
            request.Senha = string.Empty;

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == nameof(LoginRequest.Senha));
        }
    }
}
