using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;
//=========================================================
public class GameAppService : IGameAppService
{
    #region Properties
    //-------------------------------------------------
    private readonly IGameService _gameService;
    private readonly IMapper _mapper;
    //-------------------------------------------------
    #endregion

    #region Construtor
    //-------------------------------------------------
    public GameAppService(IGameService gameService, IMapper mapper)
    {
        _gameService = gameService
            ?? throw new ArgumentNullException(nameof(gameService));

        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }
    //-------------------------------------------------
    #endregion

    #region Interfaces
    //-------------------------------------------------
    public GameDtos Inserir(GameDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Game>(dto);
        var criado = _gameService.Insert(entity);

        return _mapper.Map<GameDtos>(criado);
    }
    //-------------------------------------------------
    public GameDtos Alterar(GameDtos dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        var entity = _mapper.Map<Game>(dto);
        var atualizado = _gameService.Update(entity);

        return _mapper.Map<GameDtos>(atualizado);
    }
    //-------------------------------------------------
    public GameDtos Inativar(Guid id)
    {
        //if (id == Guid.Empty)
        //    throw new ArgumentException("Id inválido.", nameof(id));

        //var entity = _contatoService.Inativar(id);

        //if (entity is null)
        //    throw new KeyNotFoundException("Contato não encontrado.");

        //return _mapper.Map<ContatosDtos>(entity);
        throw new KeyNotFoundException("Games não encontrado.");
    }
    //-------------------------------------------------
    public List<GameDtos> Listar()
    {
        var lista = _gameService.Get();
        return _mapper.Map<List<GameDtos>>(lista);
    }
    //-------------------------------------------------
    public GameDtos PorId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.", nameof(id));

        var entity = _gameService.GetById(id);

        if (entity is null)
            throw new KeyNotFoundException("Game não encontrado.");

        return _mapper.Map<GameDtos>(entity);
    }
    //-------------------------------------------------
    #endregion
}
//=========================================================
