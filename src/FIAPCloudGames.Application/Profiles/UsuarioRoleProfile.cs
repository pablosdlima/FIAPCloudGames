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
        CreateMap<AlterarUsuarioRoleRequest, UsuarioRole>();
        CreateMap<UsuarioRole, AlterarUsuarioRoleRequest>();
    }
    //---------------------------------------------------
    #endregion
}
//===============================================================
