using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Responses.Endereco;

namespace FIAPCloudGames.Api.Endpoints;

public static class EnderecoEndpoints
{
    public static void MapEnderecos(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/enderecos").WithTags("Enderecos");


        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IEnderecoAppService enderecoService) =>
        {
            var enderecos = await enderecoService.ListarPorUsuario(usuarioId);

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Endereços listados com sucesso.",
                data = enderecos
            });
        })
        .WithName("ListarEnderecosDoUsuario")
        .Produces<List<EnderecoResponse>>(200);


        app.MapPost("Cadastrar/", async (Guid usuarioId, CadastrarEnderecoRequest request, IEnderecoAppService enderecoService) =>
        {
            request = request with { UsuarioId = usuarioId };

            var endereco = await enderecoService.Cadastrar(request);

            return Results.Created($"/api/usuarios/{usuarioId}/enderecos/{endereco.Id}", new
            {
                statusCode = 201,
                message = "Endereço cadastrado com sucesso.",
                data = endereco
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarEnderecoRequest>>()
        .WithName("CadastrarEndereco")
        .Produces<EnderecoResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarEnderecoRequest request, IEnderecoAppService enderecoService) =>
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

            // Garante que pertence ao usuário correto
            request = request with { UsuarioId = usuarioId };

            var (endereco, sucesso) = await enderecoService.Atualizar(request);

            if (!sucesso || endereco == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "endereco", new[] { "Endereço não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Endereço atualizado com sucesso.",
                data = endereco
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarEnderecoRequest>>()
        .WithName("AtualizarEndereco")
        .Produces<EnderecoResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IEnderecoAppService enderecoService) =>
        {
            var sucesso = await enderecoService.Deletar(id, usuarioId);

            if (!sucesso)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "endereco", new[] { "Endereço não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Endereço removido com sucesso."
            });
        })
        .WithName("DeletarEndereco")
        .Produces(200)
        .Produces(404);
    }
}
