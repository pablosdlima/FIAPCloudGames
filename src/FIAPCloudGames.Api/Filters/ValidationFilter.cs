using FluentValidation;

namespace FIAPCloudGames.Api.Filters
{
    public static class ValidationFilter
    {
        public static async Task<IResult?> ValidateAsync<T>(
            T model,
            IValidator<T> validator) where T : class
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var responseBody = new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = "Erro de validação",
                    errors
                };
                return Results.BadRequest(responseBody);
            }
            return null;
        }
    }
}
