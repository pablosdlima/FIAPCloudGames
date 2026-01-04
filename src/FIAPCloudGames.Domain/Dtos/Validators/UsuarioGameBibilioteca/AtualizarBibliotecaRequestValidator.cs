using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.UsuarioGameBibilioteca
{
    public class AtualizarBibliotecaRequestValidator : AbstractValidator<AtualizarBibliotecaRequest>
    {
        public AtualizarBibliotecaRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório.");

            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("UsuarioId é obrigatório.");

            RuleFor(x => x.GameId)
                .NotEmpty()
                .WithMessage("GameId é obrigatório.");

            RuleFor(x => x.TipoAquisicao)
                .NotEmpty()
                .WithMessage("Tipo de aquisição é obrigatório.")
                .Must(BeAValidTipoAquisicao)
                .WithMessage("Tipo de aquisição inválido. Valores aceitos: Compra, Presente, Assinatura, Gratuito.");

            RuleFor(x => x.PrecoAquisicao)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Preço de aquisição deve ser maior ou igual a zero.");

            RuleFor(x => x.DataAquisicao)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("Data de aquisição não pode ser futura.")
                .When(x => x.DataAquisicao.HasValue);
        }

        private bool BeAValidTipoAquisicao(string tipo)
        {
            var tiposValidos = new[] { "Compra", "Presente", "Assinatura", "Gratuito" };
            return tiposValidos.Contains(tipo, StringComparer.OrdinalIgnoreCase);
        }
    }
}
