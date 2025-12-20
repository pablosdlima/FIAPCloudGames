using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//===================================================
public class EnderecoProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public EnderecoProfile()
    {
        CreateMap<EnderecoDtos, Endereco>();
        CreateMap<Endereco, EnderecoDtos>();
    }
    //---------------------------------------------------
    #endregion
}
//===================================================