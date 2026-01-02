using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Responses.Usuarios;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Mappers
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, BuscarPorIdResponse>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UsuarioRoles));

            CreateMap<UsuarioPerfil, UsuarioPerfilResponse>();

            CreateMap<UsuarioRole, UsuarioRoleResponse>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Role.Description));
        }
    }
}
