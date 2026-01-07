using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAPCloudGames.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DomainServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IContatoService, ContatoService>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<IGameService, GamesServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IUsuarioService, UsuarioServices>();
            services.AddScoped<IUsuarioGameBibliotecaService, UsuarioGameBibliotecaServices>();
            services.AddScoped<IUsuarioPerfilService, UsuarioPerfilServices>();
            services.AddScoped<IUsuarioRoleServices, UsuarioRoleServices>();

            return services;
        }
    }
}
