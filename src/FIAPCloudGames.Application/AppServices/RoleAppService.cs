using AutoMapper;
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
    private readonly IMapper _mapper;

    public RoleAppService(IRoleServices roleService, IMapper mapper)
    {
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<RolesResponse> Cadastrar(CadastrarRoleRequest request)
    {
        var entity = _mapper.Map<Role>(request);
        var criado = await _roleService.Insert(entity);

        if (criado == null)
        {
            throw new DomainException("Não foi possível cadastrar a role. Verifique os dados fornecidos.");
        }

        return _mapper.Map<RolesResponse>(criado);
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