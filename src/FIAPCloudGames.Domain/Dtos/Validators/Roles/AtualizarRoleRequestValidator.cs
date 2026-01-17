using FIAPCloudGames.Domain.Dtos.Request.Role;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Roles
{
    public class AtualizarRoleRequestValidator : AbstractValidator<AtualizarRoleRequest>
    {
        public AtualizarRoleRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id deve ser maior que zero.");

            RuleFor(x => x.RoleName)
                .NotEmpty()
                .WithMessage("Nome da role é obrigatório.")
                .MaximumLength(50)
                .WithMessage("Nome da role deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(200)
                .WithMessage("Descrição deve ter no máximo 200 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
