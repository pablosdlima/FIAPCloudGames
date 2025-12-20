using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//================================================
public static class UsuarioGameEndpoints
{
    public static void MapUsuarioGames(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/UsuarioGames").WithTags("UsuarioGames");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IUsuarioGameAppService Usuarioservice) =>
        {
            var result = Usuarioservice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (UsuarioGameBibliotecaDto dto, IUsuarioGameAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (UsuarioGameBibliotecaDto dto, IUsuarioGameAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//================================================
