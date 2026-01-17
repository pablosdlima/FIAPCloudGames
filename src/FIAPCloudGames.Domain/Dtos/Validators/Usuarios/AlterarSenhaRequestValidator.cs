using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Usuarios
{
    public class AlterarSenhaRequestValidator : AbstractValidator<AlterarSenhaRequest>
    {
        public AlterarSenhaRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do usuário é obrigatório.");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(6)
                .WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}