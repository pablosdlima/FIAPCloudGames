using AutoMapper;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.Profiles;
//=======================================================
public class RoleProfile : Profile
{
    #region Construtor
    //---------------------------------------------------
    public RoleProfile()
    {
        CreateMap<RoleDtos, Role>();
        CreateMap<Role, RoleDtos>();
    }
    //---------------------------------------------------
    #endregion
}
//=======================================================
