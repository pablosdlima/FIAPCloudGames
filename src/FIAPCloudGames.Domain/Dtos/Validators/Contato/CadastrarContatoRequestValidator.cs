using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Contato
{
    public class CadastrarContatoRequestValidator : AbstractValidator<CadastrarContatoRequest>
    {
        public CadastrarContatoRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("UsuarioId é obrigatório.");

            RuleFor(x => x.Celular)
                .NotEmpty()
                .WithMessage("Celular é obrigatório.")
                .Matches(@"^|$\d{2}$|\s?\d{4,5}-?\d{4}$")
                .WithMessage("Celular deve estar no formato (00) 00000-0000 ou (00) 0000-0000.");

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
