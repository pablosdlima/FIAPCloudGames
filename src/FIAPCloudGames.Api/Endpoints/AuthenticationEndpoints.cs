using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request;
using FIAPCloudGames.Domain.Exceptions;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthentication(this IEndpointRouteBuilder route)
        {
            var app = route.MapGroup("/api/Authentication").WithTags("Authentication");

            app.MapPost("login/", async (LoginRequest request, IAuthenticationAppService authenticationService, IValidator<LoginRequest> validator) =>
            {
                var validationError = await ValidationFilter.ValidateAsync(request, validator);
                if (validationError != null)
                {
                    return validationError;
                }

                try
                {
                    var result = await authenticationService.Login(request.Usuario, request.Senha);
                    return Results.Ok(result);
                }
                catch (AutenticacaoException)
                {
                    return Results.Unauthorized();
                }
                catch (Exception)
                {
                    return Results.Problem(
                        detail: "Erro interno no servidor.",
                        statusCode: 500
                    );
                }
            });
        }
    }
}