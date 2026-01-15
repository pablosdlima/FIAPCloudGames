using FIAPCloudGames.Api;
using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FIAPCloudGames.Presentation.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<ApiAssemblyReference>
    {
        public IAuthenticationAppService MockAuthenticationAppService { get; private set; } = default!;
        public IValidator<LoginRequest> MockLoginRequestValidator { get; private set; } = default!;
        public ILogger<Application.AppServices.AuthenticationAppService> MockAppServiceLogger { get; private set; } = default!;

        public CustomWebApplicationFactory()
        {
            Console.WriteLine("CustomWebApplicationFactory: Construtor chamado.");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                Console.WriteLine("CustomWebApplicationFactory: Configurando serviços.");

                var authenticationAppServiceDescriptor = services.FirstOrDefault(
                    d => d.ServiceType == typeof(IAuthenticationAppService));
                if (authenticationAppServiceDescriptor != null)
                {
                    services.Remove(authenticationAppServiceDescriptor);
                    Console.WriteLine("CustomWebApplicationFactory: IAuthenticationAppService real removido.");
                }

                var loginRequestValidatorDescriptor = services.FirstOrDefault(
                    d => d.ServiceType == typeof(IValidator<LoginRequest>));
                if (loginRequestValidatorDescriptor != null)
                {
                    services.Remove(loginRequestValidatorDescriptor);
                    Console.WriteLine("CustomWebApplicationFactory: IValidator<LoginRequest> real removido.");
                }

                var appServiceLoggerDescriptor = services.FirstOrDefault(
                    d => d.ServiceType == typeof(ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService>));
                if (appServiceLoggerDescriptor != null)
                {
                    services.Remove(appServiceLoggerDescriptor);
                    Console.WriteLine("CustomWebApplicationFactory: ILogger real removido.");
                }

                MockAuthenticationAppService = Substitute.For<IAuthenticationAppService>();
                MockLoginRequestValidator = Substitute.For<IValidator<LoginRequest>>();
                MockAppServiceLogger = Substitute.For<ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService>>();

                services.AddSingleton(MockAuthenticationAppService);
                services.AddSingleton(MockLoginRequestValidator);
                services.AddSingleton(MockAppServiceLogger);
                Console.WriteLine("CustomWebApplicationFactory: Mocks registrados.");

                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                });
            });

            builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(ApiAssemblyReference).Assembly.FullName);
            builder.UseEnvironment("Development");

            builder.Configure(app =>
            {
                Console.WriteLine("CustomWebApplicationFactory: Configurando pipeline de middlewares.");
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapAuthentication();
                    Console.WriteLine("CustomWebApplicationFactory: Endpoints de autenticação mapeados.");
                });
            });
        }
    }
}
