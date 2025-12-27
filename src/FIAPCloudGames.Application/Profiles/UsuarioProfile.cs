using AutoMapper;
using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//================================================
public class UsuarioProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public UsuarioProfile()
    {
        CreateMap<UsuarioDtos, Usuario>();
        CreateMap<Usuario, UsuarioDtos>();
    }
    //---------------------------------------------------
    #endregion
}
//================================================
