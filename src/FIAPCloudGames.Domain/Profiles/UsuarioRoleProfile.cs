using AutoMapper;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FIAPCloudGames.Domain.Enums;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Profiles
{
    public class UsuarioRoleProfile : Profile
    {
        public UsuarioRoleProfile()
        {
            CreateMap<AlterarUsuarioRoleRequest, UsuarioRole>()
                .ConstructUsing(src => new UsuarioRole((int)src.TipoUsuario))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdUsuarioRole))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)src.TipoUsuario))
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<UsuarioRole, AlterarUsuarioRoleRequest>()
                .ForMember(dest => dest.IdUsuarioRole, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => (TipoUsuario)src.RoleId));
        }
    }
}
