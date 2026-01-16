using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuario;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuarios(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Usuarios").WithTags("Usuarios");

        app.MapGet("BuscarPorId/{id}", (Guid id, IUsuarioAppService usuarioService) =>
        {
            try
            {
                var result = usuarioService.BuscarPorId(id);
                return ApiResponses.Ok(result, "Usuário encontrado com sucesso.");
            }
            catch (KeyNotFoundException)
            {
                return ApiResponses.NotFound("usuario", "Usuário não encontrado.");
            }

        })
        .WithName("BuscarUsuarioPorId")
        .Produces<BuscarPorIdResponse>(200)
        .Produces(404);
        // }).RequireAuthorization(policy => policy.RequireRole("usuario"));
        // }).RequireAuthorization(policy => policy.RequireRole("administrador"));
        // }).RequireAuthorization();


        app.MapPost("Cadastrar/", async (CadastrarUsuarioRequest request, IUsuarioAppService usuarioService) =>
        {
            var result = await usuarioService.Cadastrar(request);
            if (result == null)
            {
                return ApiResponses.Problem("Erro ao cadastrar o usuário.");
            }
            return ApiResponses.Created($"/api/Usuarios/{result.IdUsuario}", result, "Usuário cadastrado com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarUsuarioRequest>>()
        .WithName("CadastrarUsuario")
        .Produces<CadastrarUsuarioResponse>(201)
        .Produces(400)
        .Produces(500);


        app.MapPut("AlterarSenha/", async (AlterarSenhaRequest request, IUsuarioAppService usuarioService) =>
        {
            var sucesso = await usuarioService.AlterarSenha(request);
            if (!sucesso)
            {
                return ApiResponses.NotFound("usuario", "Usuário não encontrado ou senha atual incorreta.");
            }
            return ApiResponses.OkMessage("Senha alterada com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AlterarSenhaRequest>>()
        .WithName("AlterarSenha")
        .Produces(200)
        .Produces(400)
        .Produces(404);


        app.MapPut("AlterarStatus/", async (Guid id, IUsuarioAppService usuarioService) =>
        {
            var result = await usuarioService.AlterarStatus(id);
            if (result == null)
            {
                return ApiResponses.NotFound("usuario", "Usuário não encontrado.");
            }
            return ApiResponses.Ok(result, "Status do usuário alterado com sucesso.");
        })
        .WithName("AlterarStatus")
        .Produces<AlterarStatusResponse>(200)
        .Produces(404);
    }
}
