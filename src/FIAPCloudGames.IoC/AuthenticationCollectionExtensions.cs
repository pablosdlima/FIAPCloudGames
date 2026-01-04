using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Autenticacao;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAPCloudGames.IoC
{
    [ExcludeFromCodeCoverage]
    public static class AuthenticationCollectionExtensions
    {
        public static void AddAuthenticationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
        }
    }
}
