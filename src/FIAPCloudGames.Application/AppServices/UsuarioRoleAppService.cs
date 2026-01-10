using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioRoleAppService : IUsuarioRoleAppService
{
    private readonly IUsuarioRoleServices _usuarioRoleService;
    private readonly IUsuarioService _usuarioService;
    private readonly IRoleServices _roleService;

    public UsuarioRoleAppService(IUsuarioRoleServices usuarioRoleService, IUsuarioService usuarioService, IRoleServices roleService)
    {
        _usuarioRoleService = usuarioRoleService ?? throw new ArgumentNullException(nameof(usuarioRoleService));
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
    }

    public async Task<bool> AlterarRoleUsuario(AlterarUsuarioRoleRequest request)
    {
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");
        }

        var roleExiste = _roleService.GetByIdInt((int)request.TipoUsuario);
        if (roleExiste == null)
        {
            throw new NotFoundException($"Role com ID {(int)request.TipoUsuario} não encontrada.");
        }

        var usuarioRoleExistente = _usuarioRoleService.GetById(request.IdUsuarioRole);
        if (usuarioRoleExistente == null)
        {
            throw new NotFoundException($"Associação Usuário-Role com ID {request.IdUsuarioRole} não encontrada.");
        }

        usuarioRoleExistente.UsuarioId = request.UsuarioId;
        usuarioRoleExistente.RoleId = (int)request.TipoUsuario;

        var (updatedEntity, sucesso) = await _usuarioRoleService.Update(usuarioRoleExistente);

        if (!sucesso)
        {
            throw new DomainException("Não foi possível alterar a role do usuário. Verifique os dados fornecidos.");
        }

        return true;
    }

    public async Task<IEnumerable<ListarRolesPorUsuarioResponse>> ListarRolesPorUsuario(ListarRolePorUsuarioRequest request)
    {
        var usuarioExiste = _usuarioService.GetById(request.UsuarioId);
        if (usuarioExiste == null)
        {
            throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");
        }

        var listaRolesUsuario = await _usuarioRoleService.Get()
            .Include(ur => ur.Role)
            .Where(x => x.UsuarioId == request.UsuarioId)
            .AsNoTracking()
            .ToListAsync();

        var response = listaRolesUsuario.Select(x => new ListarRolesPorUsuarioResponse
        {
            Id = x.Id,
            UsuarioId = x.UsuarioId,
            RoleId = x.Role.Id,
            RoleName = x.Role.RoleName,
            Description = x.Role.Description
        }).ToList();

        return response;
    }
}
