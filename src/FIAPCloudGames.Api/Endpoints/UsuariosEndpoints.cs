using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuarios(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Usuarios").WithTags("Usuarios");

        app.MapGet("BuscarPorId/{id}", (Guid id, IUsuarioAppService Usuarioservice, IValidator<BuscarUsuarioPorIdRequest> validator) =>
        {
            var request = new BuscarUsuarioPorIdRequest(id);

            var result = Usuarioservice.BuscarPorId(request.Id);
            if (result == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        });
        //}).RequireAuthorization(policy => policy.RequireRole("usuario"));
        //}).RequireAuthorization(policy => policy.RequireRole("administrador"));
        //}).RequireAuthorization();

        app.MapPost("Cadastrar/", async (CadastrarUsuarioRequest request, IUsuarioAppService Usuarioservice, IValidator<CadastrarUsuarioRequest> validator) =>
        {
            var validationError = await ValidationFilter.ValidateAsync(request, validator);
            if (validationError != null)
            {
                return validationError;
            }

            var result = await Usuarioservice.Cadastrar(request);
            return result != null ? Results.Ok(result) : Results.Problem();
        });

        app.MapPut("AlterarSenha/", async (AlterarSenhaRequest request, IUsuarioAppService Usuarioservice, IValidator<AlterarSenhaRequest> validator) =>
        {
            var validationError = await ValidationFilter.ValidateAsync(request, validator);
            if (validationError != null)
            {
                return validationError;
            }

            var result = Usuarioservice.AlterarSenha(request);
            return result != null ? Results.Ok("Senha alterada com sucesso.") : Results.NotFound();
        });


        app.MapPut("AlterarStatus/", async (Guid id, IUsuarioAppService Usuarioservice) =>
        {
            var result = await Usuarioservice.AlterarStatus(id);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });
    }
}