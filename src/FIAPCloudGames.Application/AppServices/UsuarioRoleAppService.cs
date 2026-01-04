using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioRoleAppService : IUsuarioRoleAppService
{
    #region Properties

    private readonly IUsuarioRoleServices _usuarioRoleService;
    private readonly IMapper _mapper;

    #endregion

    #region Construtor

    public UsuarioRoleAppService(IUsuarioRoleServices usuarioRoleService, IMapper mapper)
    {
        _usuarioRoleService = usuarioRoleService ?? throw new ArgumentNullException(nameof(usuarioRoleService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion


    public async Task<bool> AlterarRoleUsuario(AlterarUsuarioRoleRequest request)
    {
        var entity = new UsuarioRole((int)request.TipoUsuario)
        {
            Id = request.IdUsuarioRole,
            UsuarioId = request.UsuarioId,
            RoleId = (int)request.TipoUsuario
        };

        var resultado = await _usuarioRoleService.Update(entity);

        return resultado.success;
    }

    public async Task<IEnumerable<ListarRolesPorUsuarioResponse>> ListarRolesPorUsuario(ListarRolePorUsuarioRequest request)
    {
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