using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;

namespace FIAPCloudGames.Api.Endpoints;
//===============================================
public static class RolesEndpoints
{
    public static void MapRoles(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Roles").WithTags("Roles");

        // GET por Id
        app.MapGet("PorId/{id}", (Guid id, IRoleAppService Roleservice) =>
        {
            var result = Roleservice.PorId(id);
            if (result == null)
                return Results.NotFound();

            return Results.Ok(result);
        });

        // POST
        app.MapPost("/", (RoleDtos dto, IRoleAppService Roleservice) =>
        {
            var result = Roleservice.Inserir(dto);
            return result != null ? Results.Created() : Results.Problem();
        });

        // PUT
        app.MapPut("/", (RoleDtos dto, IRoleAppService Roleservice) =>
        {
            var result = Roleservice.Alterar(dto);
            return result != null ? Results.Ok(dto) : Results.NotFound();
        });
    }
}
//===============================================
