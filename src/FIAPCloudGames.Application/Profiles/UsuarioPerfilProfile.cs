using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//================================================
public class UsuarioPerfilProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public UsuarioPerfilProfile()
    {
        CreateMap<UsuarioPerfilDto, UsuarioPerfil>();
        CreateMap<Usuario, UsuarioPerfilDto>();
    }
    //---------------------------------------------------
    #endregion
}
//================================================
