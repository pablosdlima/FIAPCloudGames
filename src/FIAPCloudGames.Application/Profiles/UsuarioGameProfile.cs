using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//===================================================
public class UsuarioGameBibliotecaGameProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public UsuarioGameBibliotecaGameProfile()
    {
        CreateMap<UsuarioGameBibliotecaDto, UsuarioGameBiblioteca>();
        CreateMap<UsuarioGameBiblioteca, UsuarioGameBibliotecaDto>();
    }
    //---------------------------------------------------
    #endregion
}
//===================================================
