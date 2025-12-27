using FluentValidation;

namespace FIAPCloudGames.Api.Filters
{
    public static class ValidationFilter
    {
        public static async Task<IResult?> ValidateAsync<T>(
            T model,
            IValidator<T> validator)
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

                var response = new
                {
                    statusCode = 400,
                    message = "Erro de validação",
                    errors = errors
                };

                return Results.BadRequest(response);
            }

            return null;
        }
    }
}
