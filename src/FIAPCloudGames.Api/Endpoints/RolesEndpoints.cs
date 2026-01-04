using FIAPCloudGames.Api.Filters;
using FIAPCloudGames.Api.Helpers;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Responses.Role;

namespace FIAPCloudGames.Api.Endpoints;

public static class RolesEndpoints
{
    public static void MapRoles(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Roles").WithTags("Roles");

        app.MapPost("Cadastrar/", async (CadastrarRoleRequest request, IRoleAppService roleService) =>
        {
            var result = await roleService.Cadastrar(request);

            if (result == null)
            {
                return ApiResponses.Problem("Erro ao cadastrar a role.");
            }

            return ApiResponses.Created($"/api/Roles/{result.Id}", result, "Role cadastrada com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarRoleRequest>>()
        .WithName("CadastrarRole")
        .Produces<RolesResponse>(201)
        .Produces(400)
        .Produces(500);


        app.MapGet("ListarRoles/", async (IRoleAppService roleService) =>
        {
            var roles = await roleService.ListarRoles();
            return ApiResponses.Ok(roles, "Roles listadas com sucesso.");
        })
        .WithName("ListarRoles")
        .Produces<List<RolesResponse>>(200);


        app.MapPut("Atualizar/{id:int}", async (int id, AtualizarRoleRequest request, IRoleAppService roleService) =>
        {
            // Garante que o Id da URL é o mesmo do body
            if (id != request.Id)
            {
                return ApiResponses.BadRequest("id", "Id da URL não corresponde ao Id do corpo da requisição.");
            }

            var (role, sucesso) = await roleService.AtualizarRole(request);

            if (!sucesso || role == null)
            {
                return ApiResponses.NotFound("role", "Role não encontrada ou não foi possível atualizar.");
            }

            return ApiResponses.Ok(role, "Role atualizada com sucesso.");
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarRoleRequest>>()
        .WithName("AtualizarRole")
        .Produces<RolesResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}