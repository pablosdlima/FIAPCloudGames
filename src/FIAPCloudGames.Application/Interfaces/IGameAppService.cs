using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Interfaces;
//================================================
public interface IGameAppService
{
    //-------------------------------------------------------
    List<GameDtos> Listar();
    //-------------------------------------------------------
    GameDtos PorId(Guid id);
    //-------------------------------------------------------
    GameDtos Inserir(GameDtos dto);
    //-------------------------------------------------------
    GameDtos Alterar(GameDtos dto);
    //-------------------------------------------------------
    GameDtos Inativar(Guid id);
    //-------------------------------------------------------
}
//================================================
