using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioGameBiblioteca;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioGameBibliotecaEndpoints
{
    public static void MapUsuarioGameBiblioteca(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/biblioteca").WithTags("Biblioteca");

        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IUsuarioGameBibliotecaAppService bibliotecaService, ILogger<Program> logger) =>
        {
            var biblioteca = await bibliotecaService.ListarPorUsuario(usuarioId);
            return ApiResponses.Ok(biblioteca, "Biblioteca listada com sucesso.");
        })
        .WithName("ListarBibliotecaDoUsuario")
        .Produces<List<BibliotecaResponse>>(200);


        app.MapPost("Comprar/", async (Guid usuarioId, ComprarGameRequest request, IUsuarioGameBibliotecaAppService bibliotecaService, ILogger<Program> logger) =>
        {
            request = request with { UsuarioId = usuarioId };
            var (biblioteca, sucesso, errorMessage) = await bibliotecaService.ComprarGame(request);
            if (!sucesso)
            {
                logger.LogWarning("Falha na compra do jogo | UsuarioId: {UsuarioId} | GameId: {GameId} | Erro: {ErrorMessage}", usuarioId, request.GameId, errorMessage);
                return ApiResponses.BadRequest("game", errorMessage ?? "Não foi possível comprar o jogo.");
            }
            return ApiResponses.Created($"/api/usuarios/{usuarioId}/biblioteca/{biblioteca!.Id}", biblioteca, "Jogo comprado e adicionado à biblioteca com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<ComprarGameRequest>>()
        .WithName("ComprarGame")
        .Produces<BibliotecaResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarBibliotecaRequest request, IUsuarioGameBibliotecaAppService bibliotecaService,
            ILogger<Program> logger) =>
        {
            if (id != request.Id)
            {
                logger.LogWarning("Id da URL não corresponde ao Id do corpo da requisição | URLId: {URLId} | Id: {Id}", id, request.Id);
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }
            request = request with { UsuarioId = usuarioId };
            var (biblioteca, sucesso) = await bibliotecaService.Atualizar(request);
            if (!sucesso || biblioteca == null)
            {
                logger.LogWarning("Item da biblioteca não encontrado ou falha na atualização | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", id, usuarioId);
                return ApiResponses.NotFound("biblioteca", "Item da biblioteca não encontrado ou não pertence ao usuário.");
            }
            return ApiResponses.Ok(biblioteca, "Item da biblioteca atualizado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarBibliotecaRequest>>()
        .WithName("AtualizarBiblioteca")
        .Produces<BibliotecaResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IUsuarioGameBibliotecaAppService bibliotecaService, ILogger<Program> logger) =>
        {
            var sucesso = await bibliotecaService.Deletar(id, usuarioId);
            if (!sucesso)
            {
                logger.LogWarning("Item da biblioteca não encontrado ou falha na exclusão | BibliotecaItemId: {BibliotecaItemId} | UsuarioId: {UsuarioId}", id, usuarioId);
                return ApiResponses.NotFound("biblioteca", "Item da biblioteca não encontrado ou não pertence ao usuário.");
            }
            return ApiResponses.OkMessage("Jogo removido da biblioteca com sucesso.");
        })
        .WithName("DeletarDaBiblioteca")
        .Produces(200)
        .Produces(404);
    }
}
