using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FluentValidation;

namespace FIAPCloudGames.Domain.Validators.UsuarioRole;

public class AlterarUsuarioRoleRequestValidator : AbstractValidator<AlterarUsuarioRoleRequest>
{
    public AlterarUsuarioRoleRequestValidator()
    {
        RuleFor(x => x.IdUsuarioRole)
            .NotEmpty()
            .WithMessage("O ID da relação usuário-role é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID da relação usuário-role não pode ser vazio.");

        RuleFor(x => x.UsuarioId)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do usuário não pode ser vazio.");

        RuleFor(x => x.TipoUsuario)
            .IsInEnum()
            .WithMessage("O tipo de usuário informado é inválido.");
    }
}
