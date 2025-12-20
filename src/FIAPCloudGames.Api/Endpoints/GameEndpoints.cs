using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//===========================================
public static class GameEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Game").WithTags("Game");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IGameAppService Gameervice) =>
        {
            var result = Gameervice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (GameDtos dto, IGameAppService Gameervice) =>
        {
            var result = Gameervice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (GameDtos dto, IGameAppService Gameervice) =>
        {
            var result = Gameervice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//===========================================
