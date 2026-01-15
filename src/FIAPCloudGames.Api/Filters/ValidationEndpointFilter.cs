using FIAPCloudGames.Api.Helpers;
using FluentValidation;

namespace FIAPCloudGames.Api.Filters;

public class ValidationEndpointFilter<TRequest> : IEndpointFilter where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Console.WriteLine($"[ValidationFilter] InvokeAsync para tipo: {typeof(TRequest).Name}");

        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request == null)
        {
            Console.WriteLine("[ValidationFilter] Request model é nulo ou não encontrado. Prosseguindo.");
            return await next(context);
        }
        Console.WriteLine($"[ValidationFilter] Request model encontrado: {request.GetType().Name}");

        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
        if (validator == null)
        {
            Console.WriteLine("[ValidationFilter] Validador não encontrado para o tipo. Prosseguindo.");
            return await next(context);
        }
        Console.WriteLine($"[ValidationFilter] Validador encontrado: {validator.GetType().Name}");

        var validationResult = await validator.ValidateAsync(request);
        Console.WriteLine($"[ValidationFilter] Resultado da validação. IsValid: {validationResult.IsValid}");

        if (!validationResult.IsValid)
        {
            Console.WriteLine("[ValidationFilter] Validação falhou. Retornando BadRequest.");
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            return ApiResponses.BadRequestMultiple("Erro de validação", errors);
        }
        Console.WriteLine("[ValidationFilter] Validação bem-sucedida. Prosseguindo.");
        return await next(context);
    }
}
