using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FluentValidation;

namespace FIAPCloudGames.Domain.Validators.UsuarioRole;

public class ListarRolePorUsuarioRequestValidator : AbstractValidator<ListarRolePorUsuarioRequest>
{
    public ListarRolePorUsuarioRequestValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do usuário não pode ser vazio.");
    }
}