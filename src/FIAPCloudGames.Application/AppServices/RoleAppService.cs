using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Responses.Role;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class RoleAppService : IRoleAppService
{
    private readonly IRoleServices _roleService;

    public RoleAppService(IRoleServices roleService)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
    }

    public async Task<RolesResponse> Cadastrar(CadastrarRoleRequest request)
    {
        var entity = new Role
        {
            Id = request.Id,
            RoleName = request.RoleName,
            Description = request.Description
        };

        var criado = await _roleService.Insert(entity);

        if (criado == null)
        {
            throw new DomainException("Não foi possível cadastrar a role. Verifique os dados fornecidos.");
        }

        return new RolesResponse
        {
            Id = criado.Id,
            RoleName = criado.RoleName,
            Description = criado.Description
        };
    }

    public async Task<List<RolesResponse>> ListarRoles()
    {
        var roles = _roleService.ListarRoles();

        var rolesResponse = roles.Select(r => new RolesResponse
        {
            Id = r.Id,
            RoleName = r.RoleName,
            Description = r.Description
        }).ToList();
        return await Task.FromResult(rolesResponse);
    }

    public async Task<(RolesResponse? Role, bool Success)> AtualizarRole(AtualizarRoleRequest request)
    {
        var role = new Role
        {
            Id = request.Id,
            RoleName = request.RoleName,
            Description = request.Description
        };
        var (roleAtualizada, sucesso) = await _roleService.AtualizarRole(role);
        if (!sucesso || roleAtualizada == null)
        {
            return (null, false);
        }
        var response = new RolesResponse
        {
            Id = roleAtualizada.Id,
            RoleName = roleAtualizada.RoleName,
            Description = roleAtualizada.Description
        };
        return (response, true);
    }
}