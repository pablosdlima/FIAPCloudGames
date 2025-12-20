using AutoMapper;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Application.Dtos;

namespace FIAPCloudGames.Application.Profiles;
//======================================================
public class ContatoProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public ContatoProfile()
    {
        CreateMap<ContatosDtos, Contato>();
        CreateMap<Contato, ContatosDtos>();
    }
    //---------------------------------------------------
    #endregion
}
//======================================================
