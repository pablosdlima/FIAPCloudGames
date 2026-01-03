using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Game").WithTags("Game");

        app.MapPost("Cadastrar/", async (CadastrarGameRequest request, IGameAppService GameService) =>
        {
            var result = await GameService.Cadastrar(request);
            return result != null ? Results.Created() : Results.Problem();
        });


        app.MapGet("BuscarPorId/{id}", (Guid id, IGameAppService GameService) =>
        {
            var result = GameService.BuscarPorId(id);
            if (result == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        });

        app.MapGet("ListarGames", async ([AsParameters] ListarGamesPaginadoRequest request, IGameAppService gameService, IValidator<ListarGamesPaginadoRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    statusCode = 400,
                    message = "Validation failed",
                    errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }

            var result = await gameService.ListarGamesPaginado(request);

            return Results.Ok(new
            {
                statusCode = 200,
                data = result
            });
        })
        .WithName("ListarGamesPaginado")
        .Produces<ListarGamesPaginadoResponse>(200)
        .Produces(400);


        // PUT
        app.MapPut("/", (CadastrarGameRequest dto, IGameAppService GameService) =>
        {
            var result = GameService.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}