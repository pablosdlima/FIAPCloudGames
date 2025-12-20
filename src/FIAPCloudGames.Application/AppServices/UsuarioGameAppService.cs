using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//============================================================
public class UsuarioGameAppService : IUsuarioGameAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IUsuarioGameService _usuarioGameService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public UsuarioGameAppService(IUsuarioGameService usuarioGame, IMapper mapper)
    {
        _usuarioGameService = usuarioGame
            ?? throw new ArgumentNullException(nameof(usuarioGame));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public UsuarioGameBibliotecaDto Inserir(UsuarioGameBibliotecaDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioGameBiblioteca>(dto);
        var criado = _usuarioGameService.Insert(entity);

        return _mapper.Map<UsuarioGameBibliotecaDto>(criado);
    }
    //-------------------------------------------------
    public UsuarioGameBibliotecaDto Alterar(UsuarioGameBibliotecaDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<UsuarioGameBiblioteca>(dto);
        var atualizado = _usuarioGameService.Update(entity);

        return _mapper.Map<UsuarioGameBibliotecaDto>(atualizado);
    }
    //-------------------------------------------------
    public UsuarioGameBibliotecaDto Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _usuarioGameService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Contato não encontrado.");

        //return _mapper.Map<UsuarioGameBibliotecaDto>(entity);
        throw new KeyNotFoundException("Contato não encontrado.");
    }
    //-------------------------------------------------
    public List<UsuarioGameBibliotecaDto> Listar()
    {
        var lista = _usuarioGameService.Get();
        return _mapper.Map<List<UsuarioGameBibliotecaDto>>(lista);
    }
    //-------------------------------------------------
    public UsuarioGameBibliotecaDto PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _usuarioGameService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Contato não encontrado.");

        return _mapper.Map<UsuarioGameBibliotecaDto>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//============================================================
