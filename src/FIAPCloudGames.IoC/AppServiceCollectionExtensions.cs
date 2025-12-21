using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Autenticacao;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FIAPCloudGames.IoC
{
    [ExcludeFromCodeCoverage]
    public static class AppServiceCollectionExtensions
    {
        public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationService, AuthenticationAppService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
        }
    }
}
