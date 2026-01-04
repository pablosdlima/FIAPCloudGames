using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators.UsuarioPerfil
{
    public class AtualizarUsuarioPerfilRequestValidator : AbstractValidator<AtualizarUsuarioPerfilRequest>
    {
        public AtualizarUsuarioPerfilRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório.");

            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .WithMessage("UsuarioId é obrigatório.");

            RuleFor(x => x.NomeCompleto)
                .NotEmpty()
                .WithMessage("Nome completo é obrigatório.")
                .MaximumLength(200)
                .WithMessage("Nome completo deve ter no máximo 200 caracteres.")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$")
                .WithMessage("Nome completo deve conter apenas letras e espaços.");

            RuleFor(x => x.DataNascimento)
                .LessThan(DateTimeOffset.UtcNow)
                .WithMessage("Data de nascimento deve ser anterior à data atual.")
                .GreaterThan(DateTimeOffset.UtcNow.AddYears(-120))
                .WithMessage("Data de nascimento inválida.")
                .When(x => x.DataNascimento.HasValue);

            RuleFor(x => x.Pais)
                .NotEmpty()
                .WithMessage("País é obrigatório.")
                .MaximumLength(100)
                .WithMessage("País deve ter no máximo 100 caracteres.");

            RuleFor(x => x.AvatarUrl)
                .NotEmpty()
                .WithMessage("URL do avatar é obrigatória.")
                .MaximumLength(500)
                .WithMessage("URL do avatar deve ter no máximo 500 caracteres.")
                .Must(BeAValidUrl)
                .WithMessage("URL do avatar inválida.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
