using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Dtos.Request;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuarios(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Usuarios").WithTags("Usuarios");

        app.MapGet("PorId/{id}", (Guid id, IUsuarioAppService Usuarioservice) =>
        {
            var result = Usuarioservice.BuscarPorId(id);
            if (result == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        }).RequireAuthorization();

        app.MapPost("cadastrar/", async (CadastrarUsuarioRequest request, IUsuarioAppService Usuarioservice, IValidator<CadastrarUsuarioRequest> validator) =>
        {
            var validationError = await ValidationFilter.ValidateAsync(request, validator);
            if (validationError != null)
            {
                return validationError;
            }

            var result = Usuarioservice.Cadastrar(request);
            return result != null ? Results.Ok(result) : Results.Problem();
        });

        app.MapPut("/", (UsuarioDtos dto, IUsuarioAppService Usuarioservice) =>
        {
            var result = Usuarioservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}