using FluentValidation;

namespace FIAPCloudGames.Api.Filters;

public class ValidationEndpointFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request == null)
        {
            return await next(context);
        }

        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>();

        if (validator == null)
        {
            return await next(context);
        }

        var validationError = await ValidationFilter.ValidateAsync(request, validator);

        if (validationError != null)
        {
            return validationError;
        }

        return await next(context);
    }
}