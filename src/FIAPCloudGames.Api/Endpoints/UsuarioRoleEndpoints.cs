using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioRoleEndpoints
{
    public static void MapUsuarioRole(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuarioRole").WithTags("UsuarioRole");

        app.MapGet("ListarRolesPorUsuario/", async (Guid usuarioId, IUsuarioRoleAppService Usuarioservice) =>
        {
            var request = new ListarRolePorUsuarioRequest(usuarioId);

            var result = await Usuarioservice.ListarRolesPorUsuario(request);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });


        app.MapPut("AlterarRoleUsuario", async (AlterarUsuarioRoleRequest request, IUsuarioRoleAppService usuarioRoleService) =>
        {
            var result = await usuarioRoleService.AlterarRoleUsuario(request);

            if (!result)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new
                    {
                        usuarioRole = new[] { "Registro não encontrado ou não foi possível atualizar." }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Role do usuário alterada com sucesso."
            });
        })
        .WithName("AlterarRoleUsuario")
        .Produces(200)
        .Produces(404);
    }
}