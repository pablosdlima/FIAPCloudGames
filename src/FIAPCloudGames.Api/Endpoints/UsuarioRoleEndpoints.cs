using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//=============================================
public static class UsuarioRoleEndpoints
{
    public static void MapUsuarioRole(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuarioRole").WithTags("UsuarioRole");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IUsuarioRoleAppService Usuarioservice) =>
        {
            var result = Usuarioservice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (UsuarioRoleDto dto, IUsuarioRoleAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (UsuarioRoleDto dto, IUsuarioRoleAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }

}
//=============================================
