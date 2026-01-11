using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
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

            app.MapPost("login/", async (LoginRequest request, IAuthenticationAppService authenticationService, HttpContext httpContext, ILogger<Program> logger) =>
            {
                var correlationId = httpContext.TraceIdentifier;

                try
                {
                    var result = await authenticationService.Login(request.Usuario, request.Senha);
                    return ApiResponses.Ok(result, "Login realizado com sucesso.");
                }
                catch (AutenticacaoException ex)
                {
                    logger.LogWarning(ex, "Falha na autenticação");
                    return ApiResponses.Unauthorized("credenciais", "Usuário ou senha inválidos.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro no login | Error: {ErrorMessage}", ex.Message);
                    return ApiResponses.Problem("Ocorreu um erro inesperado durante o login.");
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
