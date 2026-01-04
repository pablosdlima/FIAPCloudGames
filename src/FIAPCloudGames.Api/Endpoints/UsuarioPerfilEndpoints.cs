using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioPerfil;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuarioPerfilEndpoints
{
    public static void MapUsuarioPerfil(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/usuarios/{usuarioId:guid}/perfil").WithTags("UsuarioPerfil");

        app.MapGet("BuscarPorUsuarioId/", async (Guid usuarioId, IUsuarioPerfilAppService perfilService) =>
        {
            var perfil = await perfilService.BuscarPorUsuarioId(usuarioId);

            return perfil == null
                ? ApiResponses.NotFound("perfil", "Perfil não encontrado para este usuário.")
                : ApiResponses.Ok(perfil, "Perfil encontrado com sucesso.");
        })
        .WithName("BuscarPerfilDoUsuario")
        .Produces<BuscarUsuarioPerfilResponse>(200)
        .Produces(404);


        app.MapPost("Cadastrar/", async (Guid usuarioId, CadastrarUsuarioPerfilRequest request, IUsuarioPerfilAppService perfilService) =>
        {
            request = request with { UsuarioId = usuarioId };

            var perfil = await perfilService.Cadastrar(request);

            return ApiResponses.Created($"/api/usuarios/{usuarioId}/perfil/{perfil.Id}", perfil, "Perfil cadastrado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarUsuarioPerfilRequest>>()
        .WithName("CadastrarPerfil")
        .Produces<BuscarUsuarioPerfilResponse>(201)
        .Produces(400);


        app.MapPut("Atualizar/{id:guid}", async (Guid usuarioId, Guid id, AtualizarUsuarioPerfilRequest request, IUsuarioPerfilAppService perfilService) =>
        {
            if (id != request.Id)
            {
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }

            request = request with { UsuarioId = usuarioId };

            var (perfil, sucesso) = await perfilService.Atualizar(request);

            if (!sucesso || perfil == null)
            {
                return ApiResponses.NotFound("perfil", "Perfil não encontrado ou não pertence ao usuário.");
            }

            return ApiResponses.Ok(perfil, "Perfil atualizado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarUsuarioPerfilRequest>>()
        .WithName("AtualizarPerfil")
        .Produces<BuscarUsuarioPerfilResponse>(200)
        .Produces(400)
        .Produces(404);


        app.MapDelete("Deletar/{id:guid}", async (Guid usuarioId, Guid id, IUsuarioPerfilAppService perfilService) =>
        {
            var sucesso = await perfilService.Deletar(id, usuarioId);

            return !sucesso
                ? ApiResponses.NotFound("perfil", "Perfil não encontrado ou não pertence ao usuário.")
                : ApiResponses.OkMessage("Perfil removido com sucesso.");
        })
        .WithName("DeletarPerfil")
        .Produces(200)
        .Produces(404);
    }
}