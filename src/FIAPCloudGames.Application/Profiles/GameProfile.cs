using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//====================================================
public class GameProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public GameProfile()
    {
        CreateMap<CadastrarGameRequest, Game>();
        CreateMap<Game, CadastrarGameRequest>();
    }
    //---------------------------------------------------
    #endregion
}
//====================================================
