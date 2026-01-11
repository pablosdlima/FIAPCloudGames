using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;

namespace FIAPCloudGames.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Game").WithTags("Game");

        app.MapPost("Cadastrar/", async (CadastrarGameRequest request, IGameAppService gameService, ILogger<Program> logger) =>
        {
            var result = await gameService.Cadastrar(request);
            if (result == null)
            {
                logger.LogError("Erro ao cadastrar o jogo | Nome: {Nome}", request.Nome);
                return ApiResponses.Problem("Erro ao cadastrar o jogo.");
            }
            return ApiResponses.Created($"/api/Game/{result.Id}", result, "Jogo cadastrado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarGameRequest>>()
        .WithName("CadastrarGame")
        .Produces<GameResponse>(201)
        .Produces(400)
        .Produces(500);


        app.MapGet("BuscarPorId/{id}", (Guid id, IGameAppService gameService, ILogger<Program> logger) =>
        {
            var result = gameService.BuscarPorId(id);
            if (result == null)
            {
                logger.LogWarning("Jogo não encontrado | Id: {Id}", id);
                return ApiResponses.NotFound("game", "Jogo não encontrado.");
            }
            return ApiResponses.Ok(result, "Jogo encontrado com sucesso.");
        })
        .WithName("BuscarGamePorId")
        .Produces<GameResponse>(200)
        .Produces(404);


        app.MapGet("ListarGames", async ([AsParameters] ListarGamesPaginadoRequest request, IGameAppService gameService, ILogger<Program> logger) =>
        {
            var result = await gameService.ListarGamesPaginado(request);
            return ApiResponses.Ok(result, "Jogos listados com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<ListarGamesPaginadoRequest>>()
        .WithName("ListarGamesPaginado")
        .Produces<ListarGamesPaginadoResponse>(200)
        .Produces(400);


        app.MapPut("AtualizarGame/{id:guid}", async (Guid id, AtualizarGameRequest request, IGameAppService gameService, ILogger<Program> logger) =>
        {
            if (id != request.Id)
            {
                logger.LogWarning("Id da URL não corresponde ao Id do corpo da requisição | Id: {id} | RequestId: {RequestId}", id, request.Id);
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }
            var (game, sucesso) = await gameService.AtualizarGame(request);
            if (!sucesso || game == null)
            {
                logger.LogWarning("Jogo não encontrado ou falha na atualização | Id: {Id}", id);
                return ApiResponses.NotFound("game", "Jogo não encontrado ou não foi possível atualizar.");
            }
            return ApiResponses.Ok(game, "Jogo atualizado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarGameRequest>>()
        .WithName("AtualizarGame")
        .Produces<GameResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}