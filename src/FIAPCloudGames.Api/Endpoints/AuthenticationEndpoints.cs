using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Exceptions;

namespace FIAPCloudGames.Api.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthentication(this IEndpointRouteBuilder route)
        {
            var app = route.MapGroup("/api/Authentication").WithTags("Authentication");

            app.MapPost("login/", async (LoginRequest request, IAuthenticationAppService authenticationService) =>
            {
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
            })
            .AddEndpointFilter<ValidationEndpointFilter<LoginRequest>>()
            .WithName("Login")
            .Produces(200)
            .Produces(400)
            .Produces(401)
            .Produces(500);
        }
    }
}