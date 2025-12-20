using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//==========================================================
public class UsuarioPerfilAppService : IUsuarioPerfilAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IUsuarioPerfilService _usuarioPerfilService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public UsuarioPerfilAppService(IUsuarioPerfilService usuarioPerfilService, IMapper mapper)
    {
        _usuarioPerfilService = usuarioPerfilService
            ?? throw new ArgumentNullException(nameof(usuarioPerfilService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public UsuarioPerfilDto Inserir(UsuarioPerfilDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioPerfil>(dto);
        var criado = _usuarioPerfilService.Insert(entity);

        return _mapper.Map<UsuarioPerfilDto>(criado);
    }
    //-------------------------------------------------
    public UsuarioPerfilDto Alterar(UsuarioPerfilDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioPerfil>(dto);
        var atualizado = _usuarioPerfilService.Update(entity);

        return _mapper.Map<UsuarioPerfilDto>(atualizado);
    }
    //-------------------------------------------------
    public UsuarioPerfilDto Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _usuarioPerfilService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Contato não encontrado.");

        //return _mapper.Map<UsuarioPerfilDto>(entity);
        throw new KeyNotFoundException("Contato não encontrado.");
    }
    //-------------------------------------------------
    public List<UsuarioPerfilDto> Listar()
    {
        var lista = _usuarioPerfilService.Get();
        return _mapper.Map<List<UsuarioPerfilDto>>(lista);
    }
    //-------------------------------------------------
    public UsuarioPerfilDto PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _usuarioPerfilService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Contato não encontrado.");

        return _mapper.Map<UsuarioPerfilDto>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//==========================================================
