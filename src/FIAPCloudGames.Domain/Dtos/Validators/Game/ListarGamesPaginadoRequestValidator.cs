using FIAPCloudGames.Domain.Dtos.Request.Game;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.Game
{
    public class ListarGamesPaginadoRequestValidator : AbstractValidator<ListarGamesPaginadoRequest>
    {
        public ListarGamesPaginadoRequestValidator()
        {
            RuleFor(x => x.NumeroPagina)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Número da página deve ser maior que zero.");

            RuleFor(x => x.TamanhoPagina)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Tamanho da página deve ser no mínimo 1.")
                .LessThanOrEqualTo(100)
                .WithMessage("Tamanho da página deve ser no máximo 100.");

            RuleFor(x => x.Genero)
                .MaximumLength(50)
                .WithMessage("Gênero deve ter no máximo 50 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Genero));

            RuleFor(x => x.Filtro)
                .MaximumLength(100)
                .WithMessage("Filtro deve ter no máximo 100 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Filtro));
        }
    }
}