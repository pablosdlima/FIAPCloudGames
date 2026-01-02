using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//===============================================================
public class UsuarioRoleProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public UsuarioRoleProfile()
    {
        CreateMap<UsuarioRoleRequest, UsuarioRole>();
        CreateMap<UsuarioRole, UsuarioRoleRequest>();
    }
    //---------------------------------------------------
    #endregion
}
//===============================================================
