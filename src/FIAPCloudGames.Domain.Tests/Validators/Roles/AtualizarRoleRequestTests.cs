using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Validators.Roles;
using FluentAssertions;

namespace FIAPCloudGames.Domain.Tests.Validators
{
    public class AtualizarRoleRequestValidatorTests
    {
        private readonly AtualizarRoleRequestValidator _validator;

        public AtualizarRoleRequestValidatorTests()
        {
            _validator = new AtualizarRoleRequestValidator();
        }

        [Fact]
        public void Deve_ter_erro_quando_Id_for_menor_ou_igual_a_zero()
        {
            var request = new AtualizarRoleRequest
            {
                Id = 0,
                RoleName = "Usuario",
                Description = "Descrição"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Id) && e.ErrorMessage == "Id deve ser maior que zero.");
        }

        [Fact]
        public void Deve_ter_erro_quando_RoleName_estiver_vazio()
        {
            var request = new AtualizarRoleRequest
            {
                Id = 1,
                RoleName = string.Empty,
                Description = "Descrição"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.RoleName) && e.ErrorMessage == "Nome da role é obrigatório.");
        }

        [Fact]
        public void Deve_ter_erro_quando_RoleName_tiver_mais_de_50_caracteres()
        {
            var longName = new string('n', 51);
            var request = new AtualizarRoleRequest
            {
                Id = 1,
                RoleName = longName,
                Description = "Descrição"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.RoleName) && e.ErrorMessage == "Nome da role deve ter no máximo 50 caracteres.");
        }

        [Fact]
        public void Deve_ter_erro_quando_Description_tiver_mais_de_200_caracteres()
        {
            var longDesc = new string('d', 201);
            var request = new AtualizarRoleRequest
            {
                Id = 1,
                RoleName = "Administrador",
                Description = longDesc
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(request.Description) && e.ErrorMessage == "Descrição deve ter no máximo 200 caracteres.");
        }

        [Fact]
        public void Deve_ser_valido_quando_todos_campos_estiverem_corretos()
        {
            var request = new AtualizarRoleRequest
            {
                Id = 1,
                RoleName = "Administrador",
                Description = "Descrição válida"
            };

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
