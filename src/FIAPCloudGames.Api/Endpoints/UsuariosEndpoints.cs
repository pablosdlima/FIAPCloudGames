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
        });

        app.MapPost("/", async (CadastrarUsuarioRequest request, IUsuarioAppService Usuarioservice, IValidator<CadastrarUsuarioRequest> validator) =>
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

    public static class ValidationFilter
    {
        public static async Task<IResult?> ValidateAsync<T>(
            T model,
            IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var response = new
                {
                    statusCode = 400, // Use 400 para erros de validação
                    message = "Erro de validação",
                    errors = errors
                };

                return Results.BadRequest(response);
            }

            return null;
        }
    }

}