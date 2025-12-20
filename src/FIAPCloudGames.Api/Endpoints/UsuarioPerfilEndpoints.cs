using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//================================================
public static class UsuarioPerfilEndpoints
{
    public static void MapUsuariosPerfil(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuariosPerfil").WithTags("UsuariosPerfil");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IUsuarioPerfilAppService Usuarioservice) =>
        {
            var result = Usuarioservice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (UsuarioPerfilDto dto, IUsuarioPerfilAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (UsuarioPerfilDto dto, IUsuarioPerfilAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }

}
//================================================
