using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Validators.UsuarioRole;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class ListarRolePorUsuarioRequestValidatorTests
    {
        private readonly ListarRolePorUsuarioRequestValidator _validator;

        public ListarRolePorUsuarioRequestValidatorTests()
        {
            _validator = new ListarRolePorUsuarioRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_UsuarioId_for_vazio()
        {
            var request = new ListarRolePorUsuarioRequest(Guid.Empty);

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(ListarRolePorUsuarioRequest.UsuarioId) && e.ErrorMessage == "O ID do usuário é obrigatório.");
            result.Errors.Should().Contain(e => e.PropertyName == nameof(ListarRolePorUsuarioRequest.UsuarioId) && e.ErrorMessage == "O ID do usuário não pode ser vazio.");
        }

        [Fact]
        public void Deve_ser_valido_quando_UsuarioId_for_valido()
        {
            var request = new ListarRolePorUsuarioRequest(Guid.NewGuid());

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
