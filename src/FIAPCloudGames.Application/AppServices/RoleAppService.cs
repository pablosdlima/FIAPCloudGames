using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Role;
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

    public async Task<CadastrarRoleRequest> Cadastrar(CadastrarRoleRequest request)
    {
        var entity = _mapper.Map<Role>(request);
        var criado = await _roleService.Insert(entity);

        return _mapper.Map<CadastrarRoleRequest>(criado);
    }

    public CadastrarRoleRequest Alterar(CadastrarRoleRequest dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = _mapper.Map<Role>(dto);
        var atualizado = _roleService.Update(entity);

        return _mapper.Map<CadastrarRoleRequest>(atualizado);
    }

    public List<CadastrarRoleRequest> Listar()
    {
        var lista = _roleService.Get();
        return _mapper.Map<List<CadastrarRoleRequest>>(lista);
    }

    public CadastrarRoleRequest PorId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id inválido.", nameof(id));
        }

        var entity = _roleService.GetById(id);

        if (entity is null)
        {
            throw new KeyNotFoundException("role não encontrado.");
        }

        return _mapper.Map<CadastrarRoleRequest>(entity);
    }

    public async Task<List<RolesResponse>> ListarRoles()
    {
        var roles = _roleService.ListarRoles();

        // Mapeamento manual
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