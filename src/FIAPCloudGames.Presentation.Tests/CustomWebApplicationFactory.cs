using FIAPCloudGames.Api;
using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Api.Middlewares;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FIAPCloudGames.Presentation.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<ApiAssemblyReference>
    {
        public IAuthenticationAppService MockAuthenticationAppService { get; private set; } = default!;
        public IValidator<LoginRequest> MockLoginRequestValidator { get; private set; } = default!;
        public ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService> MockAppServiceLogger { get; private set; } = default!;

        public IContatoAppService MockContatoAppService { get; private set; } = default!;
        public IValidator<CadastrarContatoRequest> MockCadastrarContatoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarContatoRequest> MockAtualizarContatoRequestValidator { get; private set; } = default!;
        public ILogger<FIAPCloudGames.Application.AppServices.ContatoAppService> MockContatoAppServiceLogger { get; private set; } = default!;


        public CustomWebApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var authenticationAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAuthenticationAppService));
                if (authenticationAppServiceDescriptor != null) { services.Remove(authenticationAppServiceDescriptor); }
                MockAuthenticationAppService = Substitute.For<IAuthenticationAppService>();
                services.AddSingleton(MockAuthenticationAppService);

                var loginRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<LoginRequest>));
                if (loginRequestValidatorDescriptor != null) { services.Remove(loginRequestValidatorDescriptor); }
                MockLoginRequestValidator = Substitute.For<IValidator<LoginRequest>>();
                MockLoginRequestValidator.ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockLoginRequestValidator);

                var appServiceLoggerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ILogger<Application.AppServices.AuthenticationAppService>));
                if (appServiceLoggerDescriptor != null) { services.Remove(appServiceLoggerDescriptor); }
                MockAppServiceLogger = Substitute.For<ILogger<Application.AppServices.AuthenticationAppService>>();
                services.AddSingleton(MockAppServiceLogger);


                var contatoAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IContatoAppService));
                if (contatoAppServiceDescriptor != null) { services.Remove(contatoAppServiceDescriptor); }
                MockContatoAppService = Substitute.For<IContatoAppService>();
                services.AddSingleton(MockContatoAppService);

                var cadastrarContatoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarContatoRequest>));
                if (cadastrarContatoRequestValidatorDescriptor != null) { services.Remove(cadastrarContatoRequestValidatorDescriptor); }
                MockCadastrarContatoRequestValidator = Substitute.For<IValidator<CadastrarContatoRequest>>();
                MockCadastrarContatoRequestValidator.ValidateAsync(Arg.Any<CadastrarContatoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarContatoRequestValidator);

                var atualizarContatoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarContatoRequest>));
                if (atualizarContatoRequestValidatorDescriptor != null) { services.Remove(atualizarContatoRequestValidatorDescriptor); }
                MockAtualizarContatoRequestValidator = Substitute.For<IValidator<AtualizarContatoRequest>>();
                MockAtualizarContatoRequestValidator.ValidateAsync(Arg.Any<AtualizarContatoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarContatoRequestValidator);

                var contatoAppServiceLoggerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ILogger<Application.AppServices.ContatoAppService>));
                if (contatoAppServiceLoggerDescriptor != null) { services.Remove(contatoAppServiceLoggerDescriptor); }
                MockContatoAppServiceLogger = Substitute.For<ILogger<Application.AppServices.ContatoAppService>>();
                services.AddSingleton(MockContatoAppServiceLogger);
            });

            builder.Configure(app =>
            {
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapAuthentication();
                    endpoints.MapContatos();
                });
            });
        }
    }
}
