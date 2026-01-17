using FIAPCloudGames.Domain.Dtos.Request.Game;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Game
{
    public class AtualizarGameRequestValidator : AbstractValidator<AtualizarGameRequest>
    {
        public AtualizarGameRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório.");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório.")
                .MaximumLength(200)
                .WithMessage("Nome deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty()
                .WithMessage("Descrição é obrigatória.")
                .MaximumLength(1000)
                .WithMessage("Descrição deve ter no máximo 1000 caracteres.");

            RuleFor(x => x.Genero)
                .NotEmpty()
                .WithMessage("Gênero é obrigatório.")
                .MaximumLength(50)
                .WithMessage("Gênero deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Desenvolvedor)
                .NotEmpty()
                .WithMessage("Desenvolvedor é obrigatório.")
                .MaximumLength(200)
                .WithMessage("Desenvolvedor deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Preco)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Preço deve ser maior ou igual a zero.");

            RuleFor(x => x.DataRelease)
                .Must(data => data <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(5)))
                .WithMessage("Data de lançamento não pode ser maior que 5 anos no futuro.")
                .When(x => x.DataRelease.HasValue);
        }
    }
}