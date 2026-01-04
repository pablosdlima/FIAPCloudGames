using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;

namespace FIAPCloudGames.Api.Endpoints;

public static class ContatoEndpoints
{
    public static void MapContatos(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/contatos").WithTags("Contatos");


        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IContatoAppService contatoService) =>
        {
            var contatos = await contatoService.ListarPorUsuario(usuarioId);

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Contatos listados com sucesso.",
                data = contatos
            });
        })
        .WithName("ListarContatosDoUsuario")
        .Produces<List<ContatoResponse>>(200);


        app.MapPost("Cadastrar/", async (Guid usuarioId, CadastrarContatoRequest request, IContatoAppService contatoService) =>
        {
            request = request with { UsuarioId = usuarioId };

            var contato = await contatoService.Cadastrar(request);

            return Results.Created($"/api/usuarios/{usuarioId}/contatos/{contato.Id}", new
            {
                statusCode = 201,
                message = "Contato cadastrado com sucesso.",
                data = contato
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarContatoRequest>>()
        .WithName("CadastrarContato")
        .Produces<ContatoResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarContatoRequest request, IContatoAppService contatoService) =>
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

            var (contato, sucesso) = await contatoService.Atualizar(request);

            if (!sucesso || contato == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "contato", new[] { "Contato não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Contato atualizado com sucesso.",
                data = contato
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarContatoRequest>>()
        .WithName("AtualizarContato")
        .Produces<ContatoResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IContatoAppService contatoService) =>
        {
            var sucesso = await contatoService.Deletar(id, usuarioId);

            if (!sucesso)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "contato", new[] { "Contato não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Contato removido com sucesso."
            });
        })
        .WithName("DeletarContato")
        .Produces(200)
        .Produces(404);
    }
}