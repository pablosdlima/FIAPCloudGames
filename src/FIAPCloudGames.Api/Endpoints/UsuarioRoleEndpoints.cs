using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
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
                return ApiResponses.NotFound("roles", "Nenhuma role encontrada para este usuário.");
            }

            return ApiResponses.Ok(result, "Roles listadas com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<ListarRolePorUsuarioRequest>>()
        .WithName("ListarRolesPorUsuario")
        .Produces<List<ListarRolesPorUsuarioResponse>>(200)
        .Produces(400)
        .Produces(404);

        app.MapPut("AlterarRoleUsuario", async (AlterarUsuarioRoleRequest request, IUsuarioRoleAppService usuarioRoleService) =>
        {
            var result = await usuarioRoleService.AlterarRoleUsuario(request);

            return !result
                ? ApiResponses.NotFound("usuarioRole", "Registro não encontrado ou não foi possível atualizar.")
                : ApiResponses.OkMessage("Role do usuário alterada com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AlterarUsuarioRoleRequest>>()
        .WithName("AlterarRoleUsuario")
        .Produces(200)
        .Produces(400)
        .Produces(404);
    }
}
