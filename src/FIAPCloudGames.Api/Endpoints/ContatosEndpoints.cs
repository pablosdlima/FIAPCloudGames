using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//==============================================================
public static class ContatosEndpoints
{
    public static void MapContatos(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Contatos").WithTags("Contatos");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IContatoAppService contatoService) =>
        {
            var result = contatoService.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (ContatosDtos dto, IContatoAppService contatoService) =>
        {
            var result = contatoService.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (ContatosDtos dto, IContatoAppService contatoService) =>
        {
            var result = contatoService.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//==============================================================
