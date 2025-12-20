using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//==================================================
public static class EnderecosEndpoints
{
    public static void MapEnderecos(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Enderecos").WithTags("Enderecos");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IEnderecoAppService enderecoService) =>
        {
            var result = enderecoService.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (EnderecoDtos dto, IEnderecoAppService enderecoService) =>
        {
            var result = enderecoService.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (EnderecoDtos dto, IEnderecoAppService enderecoService) =>
        {
            var result = enderecoService.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//==================================================
