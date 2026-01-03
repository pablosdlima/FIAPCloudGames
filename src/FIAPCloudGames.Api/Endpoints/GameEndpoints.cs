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


        app.MapPut("AtualizarGame/{id:guid}", async (Guid id, AtualizarGameRequest request, IGameAppService gameService, IValidator<AtualizarGameRequest> validator) =>
        {
            // Garante que o Id da URL é o mesmo do body
            if (id != request.Id)
            {
                return Results.BadRequest(new
                {
                    statusCode = 400,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                {
                    { "id", new[] { "Id da URL não corresponde ao Id do corpo da requisição." } }
                }
                });
            }

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

            var (game, sucesso) = await gameService.AtualizarGame(request);

            if (!sucesso || game == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
            {
                { "game", new[] { "Jogo não encontrado ou não foi possível atualizar." } }
            }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Jogo atualizado com sucesso.",
                data = game
            });
        })
        .WithName("AtualizarGame")
        .Produces<GameResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}