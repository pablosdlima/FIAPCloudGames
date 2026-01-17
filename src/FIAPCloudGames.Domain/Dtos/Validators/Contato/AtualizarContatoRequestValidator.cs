using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Contato
{
    public class AtualizarContatoRequestValidator : AbstractValidator<AtualizarContatoRequest>
    {
        public AtualizarContatoRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório.");

            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("UsuarioId é obrigatório.");

            RuleFor(x => x.Celular)
                .NotEmpty()
                .WithMessage("Celular é obrigatório.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório.")
                .EmailAddress()
                .WithMessage("Email inválido.")
                .MaximumLength(100)
                .WithMessage("Email deve ter no máximo 100 caracteres.");
        }
    }
}
