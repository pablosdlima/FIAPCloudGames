using FIAPCloudGames.Api.Filters;
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
                return Results.Problem(
                    detail: "Erro ao cadastrar a role.",
                    statusCode: 500
                );
            }

            return Results.Created($"/api/Roles/{result.Id}", new
            {
                statusCode = 201,
                message = "Role cadastrada com sucesso.",
                data = result
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<CadastrarRoleRequest>>()
        .WithName("CadastrarRole")
        .Produces<RolesResponse>(201)
        .Produces(400)
        .Produces(500);

        app.MapGet("ListarRoles/", async (IRoleAppService roleService) =>
        {
            var roles = await roleService.ListarRoles();

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Roles listadas com sucesso.",
                data = roles
            });
        })
        .WithName("ListarRoles")
        .Produces<List<RolesResponse>>(200);


        app.MapPut("Atualizar/{id:int}", async (int id, AtualizarRoleRequest request, IRoleAppService roleService) =>
        {
            // Garante que o Id da URL é o mesmo do body
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

            var (role, sucesso) = await roleService.AtualizarRole(request);

            if (!sucesso || role == null)
            {
                return Results.NotFound(new
                {
                    statusCode = 404,
                    message = "Validation failed",
                    errors = new Dictionary<string, string[]>
                    {
                        { "role", new[] { "Role não encontrada ou não foi possível atualizar." } }
                    }
                });
            }

            return Results.Ok(new
            {
                statusCode = 200,
                message = "Role atualizada com sucesso.",
                data = role
            });
        })
        .AddEndpointFilter<ValidationEndpointFilter<AtualizarRoleRequest>>()
        .WithName("AtualizarRole")
        .Produces<RolesResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}