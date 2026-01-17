using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Validators.UsuarioRole;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AlterarUsuarioRoleRequestValidatorTests
    {
        private readonly AlterarUsuarioRoleRequestValidator _validator;

        public AlterarUsuarioRoleRequestValidatorTests()
        {
            _validator = new AlterarUsuarioRoleRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_IdUsuarioRole_for_vazio()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.Empty,
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = TipoUsuario.Usuario
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.IdUsuarioRole) && e.ErrorMessage == "O ID da relação usuário-role é obrigatório.");
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.IdUsuarioRole) && e.ErrorMessage == "O ID da relação usuário-role não pode ser vazio.");
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_for_vazio()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.Empty,
                TipoUsuario = TipoUsuario.Usuario
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "O ID do usuário é obrigatório.");
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.UsuarioId) && e.ErrorMessage == "O ID do usuário não pode ser vazio.");
        }

        [Fact]
        public void Deve_ter_erro_quando_TipoUsuario_for_invalido()
        {
            var invalidValue = (TipoUsuario)999;
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = invalidValue
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.TipoUsuario) && e.ErrorMessage == "O tipo de usuário informado é inválido.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new AlterarUsuarioRoleRequest
            {
                IdUsuarioRole = Guid.NewGuid(),
                UsuarioId = Guid.NewGuid(),
                TipoUsuario = TipoUsuario.Administrador
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
