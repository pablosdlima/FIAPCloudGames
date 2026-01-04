using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioRoleEndpoints
{
    public static void MapUsuarioRole(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuarioRole").WithTags("UsuarioRole");


        app.MapGet("ListarRolesPorUsuario/", async (Guid usuarioId, IUsuarioRoleAppService usuarioService) =>
        {
            var request = new ListarRolePorUsuarioRequest(usuarioId);

            var result = await usuarioService.ListarRolesPorUsuario(request);

            if (result == null || !result.Any())
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "roles", new[] { "Nenhuma role encontrada para este usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Roles listadas com sucesso.",
                data = result
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<ListarRolePorUsuarioRequest>>()
        .WithName("ListarRolesPorUsuario")
        .Produces<List<ListarRolesPorUsuarioResponse>>(200)
        .Produces(400)
        .Produces(404);


        app.MapPut("AlterarRoleUsuario", async (AlterarUsuarioRoleRequest request, IUsuarioRoleAppService usuarioRoleService) =>
        {
            var result = await usuarioRoleService.AlterarRoleUsuario(request);

            if (!result)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "usuarioRole", new[] { "Registro não encontrado ou não foi possível atualizar." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Role do usuário alterada com sucesso."
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AlterarUsuarioRoleRequest>>()
        .WithName("AlterarRoleUsuario")
        .Produces(200)
        .Produces(400)
        .Produces(404);
    }
}
