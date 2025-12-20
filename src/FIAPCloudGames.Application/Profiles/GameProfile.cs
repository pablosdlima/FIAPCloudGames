using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//====================================================
public class GameProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public GameProfile()
    {
        CreateMap<GameDtos, Game>();
        CreateMap<Game, GameDtos>();
    }
    //---------------------------------------------------
    #endregion
}
//====================================================
