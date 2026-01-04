using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioGameBiblioteca;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioGameBibiotecaEndpoints
{
    public static void MapUsuarioGameBIbioteca(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/biblioteca").WithTags("Biblioteca");


        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            var biblioteca = await bibliotecaService.ListarPorUsuario(usuarioId);

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Biblioteca listada com sucesso.",
                data = biblioteca
            });
        })
        .WithName("ListarBibliotecaDoUsuario")
        .Produces<List<BibliotecaResponse>>(200);


        app.MapPost("Comprar/", async (Guid usuarioId, ComprarGameRequest request, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            request = request with { UsuarioId = usuarioId };

            var (biblioteca, sucesso, errorMessage) = await bibliotecaService.ComprarGame(request);

            if (!sucesso)
            {
                return Results.BadRequest(new
                {
                    statusCode = 400,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "game", new[] { errorMessage ?? "Não foi possível comprar o jogo." } }
                    }
                });
            }

            return Results.Created($"/api/usuarios/{usuarioId}/biblioteca/{biblioteca!.Id}", new
            {
                statusCode = 201,
                message = "Jogo comprado e adicionado à biblioteca com sucesso.",
                data = biblioteca
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<ComprarGameRequest>>()
        .WithName("ComprarGame")
        .Produces<BibliotecaResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarBibliotecaRequest request, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
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

            request = request with { UsuarioId = usuarioId };

            var (biblioteca, sucesso) = await bibliotecaService.Atualizar(request);

            if (!sucesso || biblioteca == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "biblioteca", new[] { "Item da biblioteca não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Item da biblioteca atualizado com sucesso.",
                data = biblioteca
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarBibliotecaRequest>>()
        .WithName("AtualizarBiblioteca")
        .Produces<BibliotecaResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IUsuarioGameBibliotecaAppService bibliotecaService) =>
        {
            var sucesso = await bibliotecaService.Deletar(id, usuarioId);

            if (!sucesso)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "biblioteca", new[] { "Item da biblioteca não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Jogo removido da biblioteca com sucesso."
            });
        })
        .WithName("DeletarDaBiblioteca")
        .Produces(200)
        .Produces(404);
    }
}
