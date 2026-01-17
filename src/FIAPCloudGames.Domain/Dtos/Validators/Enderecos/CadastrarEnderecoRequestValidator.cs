using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Enderecos
{
    public class CadastrarEnderecoRequestValidator : AbstractValidator<CadastrarEnderecoRequest>
    {
        public CadastrarEnderecoRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("UsuarioId é obrigatório.");

            RuleFor(x => x.Rua)
                .NotEmpty()
                .WithMessage("Rua é obrigatória.")
                .MaximumLength(200)
                .WithMessage("Rua deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Numero)
                .NotEmpty()
                .WithMessage("Número é obrigatório.")
                .MaximumLength(20)
                .WithMessage("Número deve ter no máximo 20 caracteres.");

            RuleFor(x => x.Complemento)
                .MaximumLength(100)
                .WithMessage("Complemento deve ter no máximo 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Complemento));

            RuleFor(x => x.Bairro)
                .NotEmpty()
                .WithMessage("Bairro é obrigatório.")
                .MaximumLength(100)
                .WithMessage("Bairro deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Cidade)
                .NotEmpty()
                .WithMessage("Cidade é obrigatória.")
                .MaximumLength(100)
                .WithMessage("Cidade deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("Estado é obrigatório.")
                .Length(2)
                .WithMessage("Estado deve ter 2 caracteres (UF).");

            RuleFor(x => x.Cep)
                .NotEmpty()
                .WithMessage("CEP é obrigatório.")
                .Matches(@"^\d{5}-?\d{3}$")
                .WithMessage("CEP deve estar no formato 00000-000 ou 00000000.");
        }
    }
}