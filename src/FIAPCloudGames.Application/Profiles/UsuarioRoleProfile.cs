using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//===============================================================
public class UsuarioRoleProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public UsuarioRoleProfile()
    {
        CreateMap<UsuarioRoleDto, UsuarioRole>();
        CreateMap<UsuarioRole, UsuarioRoleDto>();
    }
    //---------------------------------------------------
    #endregion
}
//===============================================================
