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


    public UsuarioRoleRequest Inserir(UsuarioRoleRequest dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = _mapper.Map<UsuarioRole>(dto);
        var criado = _usuarioRoleService.Insert(entity);

        return _mapper.Map<UsuarioRoleRequest>(criado);
    }

    public UsuarioRoleRequest Alterar(UsuarioRoleRequest dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = _mapper.Map<UsuarioRole>(dto);
        var atualizado = _usuarioRoleService.Update(entity);

        return _mapper.Map<UsuarioRoleRequest>(atualizado);
    }
    public List<UsuarioRoleRequest> Listar()
    {
        var lista = _usuarioRoleService.Get();
        return _mapper.Map<List<UsuarioRoleRequest>>(lista);
    }

    public async Task<IEnumerable<ListarRolesPorUsuarioResponse>> ListarRolesPorUsuario(ListarRolePorUsuarioRequest request)
    {
        var listaRolesUsuario = await _usuarioRoleService.Get()
            .Include(ur => ur.Role)
            .Where(x => x.UsuarioId == request.UsuarioId)
            .ToListAsync();

        return _mapper.Map<List<ListarRolesPorUsuarioResponse>>(listaRolesUsuario);
    }


    public UsuarioRoleRequest PorId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id inválido.", nameof(id));
        }

        var entity = _usuarioRoleService.GetById(id);

        if (entity is null)
        {
            throw new KeyNotFoundException("UsuarioRole não encontrado.");
        }

        return _mapper.Map<UsuarioRoleRequest>(entity);
    }
}