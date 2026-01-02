using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators
{
    public class CadastrarUsuarioRequestValidator : AbstractValidator<CadastrarUsuarioRequest>
    {
        public CadastrarUsuarioRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
                .WithMessage("Senha deve conter pelo menos: 1 letra maiúscula, 1 letra minúscula, 1 número e 1 caractere especial");


            RuleFor(x => x.Celular)
                .NotEmpty().WithMessage("Celular é obrigatório")
                .Matches(@"^\d{10,11}$").WithMessage("Celular deve ter 10 ou 11 dígitos");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.TipoUsuario)
                .IsInEnum().WithMessage("Tipo de usuário inválido");
        }
    }
}
