using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//=======================================================
public class RoleProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public RoleProfile()
    {
        CreateMap<CadastrarRoleRequest, Role>();
        CreateMap<Role, CadastrarRoleRequest>();
    }
    //---------------------------------------------------
    #endregion
}
//=======================================================
