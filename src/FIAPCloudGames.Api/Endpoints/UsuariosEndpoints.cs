using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//===============================================
public static class UsuariosEndpoints
{
    public static void MapUsuarios(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Usuarios").WithTags("Usuarios");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IUsuarioAppService Usuarioservice) =>
        {
            var result = Usuarioservice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (UsuarioDtos dto, IUsuarioAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (UsuarioDtos dto, IUsuarioAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//===============================================
