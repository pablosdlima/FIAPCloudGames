using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioRoleEndpoints
{
    public static void MapUsuarioRole(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuarioRole").WithTags("UsuarioRole");

        app.MapPut("AlterarRoleUsuario/", (UsuarioRoleDto dto, IUsuarioRoleAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}