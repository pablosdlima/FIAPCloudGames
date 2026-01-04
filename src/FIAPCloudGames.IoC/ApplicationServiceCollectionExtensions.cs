using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAPCloudGames.IoC
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IContatoAppService, ContatoAppService>();
            services.AddScoped<IEnderecoAppService, EnderecoAppService>();
            services.AddScoped<IGameAppService, GameAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IUsuarioGameBibliotecaAppService, UsuarioGameBibliotecaAppService>();
            services.AddScoped<IUsuarioPerfilAppService, UsuarioPerfilAppService>();
            services.AddScoped<IUsuarioRoleAppService, UsuarioRoleAppService>();

            return services;
        }
    }
}