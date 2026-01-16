using FIAPCloudGames.Api;
using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Api.Middlewares;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Request.Role; // Adicionar este using
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
        // Mocks expostos para que os testes possam configurá-los
        public IAuthenticationAppService MockAuthenticationAppService { get; private set; } = default!;
        public IValidator<LoginRequest> MockLoginRequestValidator { get; private set; } = default!;
        public ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService> MockAppServiceLogger { get; private set; } = default!;
        public IContatoAppService MockContatoAppService { get; private set; } = default!;
        public IValidator<CadastrarContatoRequest> MockCadastrarContatoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarContatoRequest> MockAtualizarContatoRequestValidator { get; private set; } = default!;
        public ILogger<FIAPCloudGames.Application.AppServices.ContatoAppService> MockContatoAppServiceLogger { get; private set; } = default!;
        public IEnderecoAppService MockEnderecoAppService { get; private set; } = default!;
        public IValidator<CadastrarEnderecoRequest> MockCadastrarEnderecoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarEnderecoRequest> MockAtualizarEnderecoRequestValidator { get; private set; } = default!;
        public IRoleAppService MockRoleAppService { get; private set; } = default!;
        public IValidator<CadastrarRoleRequest> MockCadastrarRoleRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarRoleRequest> MockAtualizarRoleRequestValidator { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Console.WriteLine("CustomWebApplicationFactory: ConfigureWebHost iniciado.");

            builder.ConfigureServices(services =>
            {
                Console.WriteLine("CustomWebApplicationFactory: Configurando serviços...");

                // Mocks para Authentication
                var authAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IAuthenticationAppService));
                if (authAppServiceDescriptor != null) { services.Remove(authAppServiceDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IAuthenticationAppService."); }
                MockAuthenticationAppService = Substitute.For<IAuthenticationAppService>();
                services.AddSingleton(MockAuthenticationAppService);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IAuthenticationAppService.");

                var loginRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<LoginRequest>));
                if (loginRequestValidatorDescriptor != null) { services.Remove(loginRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<LoginRequest>."); }
                MockLoginRequestValidator = Substitute.For<IValidator<LoginRequest>>();
                MockLoginRequestValidator.ValidateAsync(Arg.Any<LoginRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockLoginRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<LoginRequest>.");

                var appServiceLoggerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService>));
                if (appServiceLoggerDescriptor != null) { services.Remove(appServiceLoggerDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido ILogger<AuthenticationAppService>."); }
                MockAppServiceLogger = Substitute.For<ILogger<FIAPCloudGames.Application.AppServices.AuthenticationAppService>>();
                services.AddSingleton(MockAppServiceLogger);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock ILogger<AuthenticationAppService>.");

                // Mocks para Contato
                var contatoAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IContatoAppService));
                if (contatoAppServiceDescriptor != null) { services.Remove(contatoAppServiceDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IContatoAppService."); }
                MockContatoAppService = Substitute.For<IContatoAppService>();
                services.AddSingleton(MockContatoAppService);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IContatoAppService.");

                var cadastrarContatoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarContatoRequest>));
                if (cadastrarContatoRequestValidatorDescriptor != null) { services.Remove(cadastrarContatoRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<CadastrarContatoRequest>."); }
                MockCadastrarContatoRequestValidator = Substitute.For<IValidator<CadastrarContatoRequest>>();
                MockCadastrarContatoRequestValidator.ValidateAsync(Arg.Any<CadastrarContatoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarContatoRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<CadastrarContatoRequest>.");

                var atualizarContatoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarContatoRequest>));
                if (atualizarContatoRequestValidatorDescriptor != null) { services.Remove(atualizarContatoRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<AtualizarContatoRequest>."); }
                MockAtualizarContatoRequestValidator = Substitute.For<IValidator<AtualizarContatoRequest>>();
                MockAtualizarContatoRequestValidator.ValidateAsync(Arg.Any<AtualizarContatoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarContatoRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<AtualizarContatoRequest>.");

                var contatoAppServiceLoggerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ILogger<FIAPCloudGames.Application.AppServices.ContatoAppService>));
                if (contatoAppServiceLoggerDescriptor != null) { services.Remove(contatoAppServiceLoggerDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido ILogger<ContatoAppService>."); }
                MockContatoAppServiceLogger = Substitute.For<ILogger<FIAPCloudGames.Application.AppServices.ContatoAppService>>();
                services.AddSingleton(MockContatoAppServiceLogger);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock ILogger<ContatoAppService>.");

                // Mocks para Endereço
                var enderecoAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IEnderecoAppService));
                if (enderecoAppServiceDescriptor != null) { services.Remove(enderecoAppServiceDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IEnderecoAppService."); }
                MockEnderecoAppService = Substitute.For<IEnderecoAppService>();
                services.AddSingleton(MockEnderecoAppService);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IEnderecoAppService.");

                var cadastrarEnderecoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarEnderecoRequest>));
                if (cadastrarEnderecoRequestValidatorDescriptor != null) { services.Remove(cadastrarEnderecoRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<CadastrarEnderecoRequest>."); }
                MockCadastrarEnderecoRequestValidator = Substitute.For<IValidator<CadastrarEnderecoRequest>>();
                MockCadastrarEnderecoRequestValidator.ValidateAsync(Arg.Any<CadastrarEnderecoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarEnderecoRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<CadastrarEnderecoRequest>.");

                var atualizarEnderecoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarEnderecoRequest>));
                if (atualizarEnderecoRequestValidatorDescriptor != null) { services.Remove(atualizarEnderecoRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<AtualizarEnderecoRequest>."); }
                MockAtualizarEnderecoRequestValidator = Substitute.For<IValidator<AtualizarEnderecoRequest>>();
                MockAtualizarEnderecoRequestValidator.ValidateAsync(Arg.Any<AtualizarEnderecoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarEnderecoRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<AtualizarEnderecoRequest>.");

                // Mocks para Role
                var roleAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IRoleAppService));
                if (roleAppServiceDescriptor != null) { services.Remove(roleAppServiceDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IRoleAppService."); }
                MockRoleAppService = Substitute.For<IRoleAppService>();
                services.AddSingleton(MockRoleAppService);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IRoleAppService.");

                var cadastrarRoleRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarRoleRequest>));
                if (cadastrarRoleRequestValidatorDescriptor != null) { services.Remove(cadastrarRoleRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<CadastrarRoleRequest>."); }
                MockCadastrarRoleRequestValidator = Substitute.For<IValidator<CadastrarRoleRequest>>();
                MockCadastrarRoleRequestValidator.ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarRoleRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<CadastrarRoleRequest>.");

                var atualizarRoleRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarRoleRequest>));
                if (atualizarRoleRequestValidatorDescriptor != null) { services.Remove(atualizarRoleRequestValidatorDescriptor); Console.WriteLine("CustomWebApplicationFactory: Removido IValidator<AtualizarRoleRequest>."); }
                MockAtualizarRoleRequestValidator = Substitute.For<IValidator<AtualizarRoleRequest>>();
                MockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarRoleRequestValidator);
                Console.WriteLine("CustomWebApplicationFactory: Adicionado mock IValidator<AtualizarRoleRequest>.");
            });

            // Substituir o método Configure para mapear APENAS os endpoints de Roles
            builder.Configure(app =>
            {
                Console.WriteLine("CustomWebApplicationFactory: Configurando pipeline de requisição (APENAS ROLES)...");
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    Console.WriteLine("CustomWebApplicationFactory: Mapeando endpoints...");
                    endpoints.MapRoles();
                    Console.WriteLine("CustomWebApplicationFactory: Endpoints de Roles mapeados.");
                    // Comentar ou remover os outros mapeamentos para evitar inicialização de dependências não mockadas
                    endpoints.MapAuthentication();
                    endpoints.MapContatos();
                    endpoints.MapEnderecos();
                });
                Console.WriteLine("CustomWebApplicationFactory: Pipeline de requisição de Roles configurado.");
            });
            Console.WriteLine("CustomWebApplicationFactory: ConfigureWebHost concluído.");
        }
    }
}
