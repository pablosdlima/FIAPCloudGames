using FIAPCloudGames.Domain.Dtos.Request;
using FluentValidation;

namespace FIAPCloudGames.Domain.Dtos.Validators
{
    public class BuscarUsuarioPorIdRequestValidator : AbstractValidator<BuscarUsuarioPorIdRequest>
    {
        public BuscarUsuarioPorIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do usuário é obrigatório.");
        }
    }
}
