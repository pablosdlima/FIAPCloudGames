using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FluentValidation;

namespace FIAPCloudGames.Api.Endpoints;

public static class RolesEndpoints
{
    public static void MapRoles(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/api/Roles").WithTags("Roles");

        app.MapPost("Cadastrar/", async (CadastrarRoleRequest request, IRoleAppService Roleservice) =>
        {
            var result = await Roleservice.Cadastrar(request);
            return result != null ? Results.Created() : Results.Problem();
        });


        app.MapGet("ListarRoles/", async (IRoleAppService roleService) =>
        {
            var roles = await roleService.ListarRoles();

            return Results.Ok(new
            {
                statusCode = 200,
                data = roles
            });
        })
        .WithName("ListarRoles")
        .Produces<List<RolesResponse>>(200);


        app.MapPut("Atualizar/{id:int}", async (int id, AtualizarRoleRequest request, IRoleAppService roleService, IValidator<AtualizarRoleRequest> validator) =>
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

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new
                {
                    statusCode = 400,
                    message = "Validation failed",
                    errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
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
                data = role
            });
        })
        .WithName("AtualizarRole")
        .Produces<RolesResponse>(200)
        .Produces(400)
        .Produces(404);
    }
}