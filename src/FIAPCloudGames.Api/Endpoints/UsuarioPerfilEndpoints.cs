using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioPerfilEndpoints
{
    public static void MapUsuarioPerfil(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/perfil").WithTags("UsuarioPerfil");

        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IUsuarioPerfilAppService perfilService) =>
        {
            var perfil = await perfilService.BuscarPorUsuarioId(usuarioId);

            if (perfil == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "perfil", new[] { "Perfil não encontrado para este usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Perfil encontrado com sucesso.",
                data = perfil
            });
        })
        .WithName("BuscarPerfilDoUsuario")
        .Produces<UsuarioPerfilResponse>(200)
        .Produces(404);


        app.MapPost("Cadastrar/", async (Guid usuarioId, CadastrarUsuarioPerfilRequest request, IUsuarioPerfilAppService perfilService, IValidator<CadastrarUsuarioPerfilRequest> validator) =>
        {
            request = request with { UsuarioId = usuarioId };

            var validationError = await ValidationFilter.ValidateAsync(request, validator);
            if (validationError != null)
            {
                return validationError;
            }

            var perfil = await perfilService.Cadastrar(request);

            return Results.Created($"/api/usuarios/{usuarioId}/perfil/{perfil.Id}", new
            {
                statusCode = 201,
                message = "Perfil cadastrado com sucesso.",
                data = perfil
            });
        })
        .WithName("CadastrarPerfil")
        .Produces<UsuarioPerfilResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarUsuarioPerfilRequest request, IUsuarioPerfilAppService perfilService, IValidator<AtualizarUsuarioPerfilRequest> validator) =>
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

            var validationError = await ValidationFilter.ValidateAsync(request, validator);
            if (validationError != null)
            {
                return validationError;
            }

            var (perfil, sucesso) = await perfilService.Atualizar(request);

            if (!sucesso || perfil == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "perfil", new[] { "Perfil não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Perfil atualizado com sucesso.",
                data = perfil
            });
        })
        .WithName("AtualizarPerfil")
        .Produces<UsuarioPerfilResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IUsuarioPerfilAppService perfilService) =>
        {
            var sucesso = await perfilService.Deletar(id, usuarioId);

            if (!sucesso)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "perfil", new[] { "Perfil não encontrado ou não pertence ao usuário." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Perfil removido com sucesso."
            });
        })
        .WithName("DeletarPerfil")
        .Produces(200)
        .Produces(404);
    }
}
