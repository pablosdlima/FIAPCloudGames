using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("Usuario é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatório");
        }
    }
}
