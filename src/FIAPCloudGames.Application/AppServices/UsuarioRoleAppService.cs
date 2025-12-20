using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//=================================================
public class UsuarioRoleAppService : IUsuarioRoleAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IUsuarioRoleServices _usuarioRoleService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public UsuarioRoleAppService(IUsuarioRoleServices usuarioRoleService, IMapper mapper)
    {
        _usuarioRoleService = usuarioRoleService
            ?? throw new ArgumentNullException(nameof(usuarioRoleService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public UsuarioRoleDto Inserir(UsuarioRoleDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioRole>(dto);
        var criado = _usuarioRoleService.Insert(entity);

        return _mapper.Map<UsuarioRoleDto>(criado);
    }
    //-------------------------------------------------
    public UsuarioRoleDto Alterar(UsuarioRoleDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioRole>(dto);
        var atualizado = _usuarioRoleService.Update(entity);

        return _mapper.Map<UsuarioRoleDto>(atualizado);
    }
    //-------------------------------------------------
    public UsuarioRoleDto Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _usuarioRoleService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("UsuarioRole não encontrado.");

        //return _mapper.Map<UsuarioRoleDto>(entity);
        throw new KeyNotFoundException("UsuarioRole não encontrado.");
    }
    //-------------------------------------------------
    public List<UsuarioRoleDto> Listar()
    {
        var lista = _usuarioRoleService.Get();
        return _mapper.Map<List<UsuarioRoleDto>>(lista);
    }
    //-------------------------------------------------
    public UsuarioRoleDto PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _usuarioRoleService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("UsuarioRole não encontrado.");

        return _mapper.Map<UsuarioRoleDto>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//=================================================
