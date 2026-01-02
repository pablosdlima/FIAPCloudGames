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


        app.MapPut("AlterarRoleUsuario/", (UsuarioRoleRequest dto, IUsuarioRoleAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}