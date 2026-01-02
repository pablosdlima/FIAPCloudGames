using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Profiles
{
    public class AlterarUsuarioRoleProfile : Profile
    {
        public AlterarUsuarioRoleProfile()
        {
            CreateMap<UsuarioRole, AlterarUsuarioRoleRequest>()
                .ForMember(dest => dest.IdUsuarioRole, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.RoleId));
        }
    }
}
