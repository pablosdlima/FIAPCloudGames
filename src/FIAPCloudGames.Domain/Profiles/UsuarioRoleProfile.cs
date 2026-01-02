using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioRole;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Profiles
{
    public class UsuarioRoleProfile : Profile
    {
        public UsuarioRoleProfile()
        {
            CreateMap<UsuarioRole, ListarRolesPorUsuarioResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Role.Description));
        }
    }
}
