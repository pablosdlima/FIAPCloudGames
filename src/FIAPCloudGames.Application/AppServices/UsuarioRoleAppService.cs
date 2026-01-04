using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioRoleAppService : IUsuarioRoleAppService
{
    #region Properties
    private readonly IUsuarioRoleServices _usuarioRoleService;
    private readonly IUsuarioService _usuarioService;
    private readonly IRoleServices _roleService;
    private readonly IMapper _mapper;
    #endregion

    #region Construtor
    public UsuarioRoleAppService(
        IUsuarioRoleServices usuarioRoleService,
        IUsuarioService usuarioService,
        IRoleServices roleService,
        IMapper mapper)
    {
        _usuarioRoleService = usuarioRoleService ?? throw new ArgumentNullException(nameof(usuarioRoleService));
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    #endregion

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

        var entity = new UsuarioRole((int)request.TipoUsuario)
        {
            Id = request.IdUsuarioRole,
            UsuarioId = request.UsuarioId,
            RoleId = (int)request.TipoUsuario
        };

        var (updatedEntity, sucesso) = await _usuarioRoleService.Update(entity);

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