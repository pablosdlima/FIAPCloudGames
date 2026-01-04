using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuarios(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Usuarios")
            .WithTags("Usuarios");

        // GET - Sem validação (ID vem da rota)
        app.MapGet("BuscarPorId/{id}", (
            Guid id,
            IUsuarioAppService usuarioService) =>
        {
            try
            {
                var result = usuarioService.BuscarPorId(id);

                return Results.Ok(new
                {
                    statusCode = 200,
                    message = "Usuário encontrado com sucesso.",
                    data = result
                });
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "usuario", new[] { "Usuário não encontrado." } }
                    }
                });
            }
        })
        .WithName("BuscarUsuarioPorId")
        .Produces<BuscarPorIdResponse>(200)
        .Produces(404);
        // }).RequireAuthorization(policy => policy.RequireRole("usuario"));
        // }).RequireAuthorization(policy => policy.RequireRole("administrador"));
        // }).RequireAuthorization();

        // POST - Com validação automática
        app.MapPost("Cadastrar/", async (
            CadastrarUsuarioRequest request,
            IUsuarioAppService usuarioService) =>
        {
            var result = await usuarioService.Cadastrar(request);

            if (result == null)
            {
                return Results.Problem(
                    detail: "Erro ao cadastrar o usuário.",
                    statusCode: 500
                );
            }

            return Results.Created($"/api/Usuarios/{result.IdUsuario}", new
            {
                statusCode = 201,
                message = "Usuário cadastrado com sucesso.",
                data = result
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarUsuarioRequest>>()
        .WithName("CadastrarUsuario")
        .Produces<CadastrarUsuarioResponse>(201)
        .Produces(400)
        .Produces(500);

        // PUT - Com validação automática
        app.MapPut("AlterarSenha/", async (
            AlterarSenhaRequest request,
            IUsuarioAppService usuarioService) =>
        {
            var sucesso = await usuarioService.AlterarSenha(request);

            if (!sucesso)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "usuario", new[] { "Usuário não encontrado ou senha atual incorreta." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Senha alterada com sucesso."
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AlterarSenhaRequest>>()
        .WithName("AlterarSenha")
        .Produces(200)
        .Produces(400)
        .Produces(404);

        // PUT - Sem validação (apenas route parameter)
        app.MapPut("AlterarStatus/", async (
            Guid id,
            IUsuarioAppService usuarioService) =>
        {
            var result = await usuarioService.AlterarStatus(id);

            if (result == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "usuario", new[] { "Usuário não encontrado." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Status do usuário alterado com sucesso.",
                data = result
            });
        })
        .WithName("ToggleUsuarioStatus")
        .Produces<AlterarStatusResponse>(200)
        .Produces(404);
    }
}
