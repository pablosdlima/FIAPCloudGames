using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//==========================================================
public class UsuarioAppService : IUsuarioAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IUsuarioService _UsuarioService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public UsuarioAppService(IUsuarioService usuarioService, IMapper mapper)
    {
        _UsuarioService = usuarioService
            ?? throw new ArgumentNullException(nameof(usuarioService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public UsuarioDtos Inserir(UsuarioDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Usuario>(dto);
        var criado = _UsuarioService.Insert(entity);

        return _mapper.Map<UsuarioDtos>(criado);
    }
    //-------------------------------------------------
    public UsuarioDtos Alterar(UsuarioDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Usuario>(dto);
        var atualizado = _UsuarioService.Update(entity);

        return _mapper.Map<UsuarioDtos>(atualizado);
    }
    //-------------------------------------------------
    public UsuarioDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _UsuarioService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Usuario não encontrado.");

        //return _mapper.Map<UsuarioDtos>(entity);
        throw new KeyNotFoundException("Usuario não encontrado.");
    }
    //-------------------------------------------------
    public List<UsuarioDtos> Listar()
    {
        var lista = _UsuarioService.Get();
        return _mapper.Map<List<UsuarioDtos>>(lista);
    }
    //-------------------------------------------------
    public UsuarioDtos PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _UsuarioService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Usuario não encontrado.");

        return _mapper.Map<UsuarioDtos>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//==========================================================
