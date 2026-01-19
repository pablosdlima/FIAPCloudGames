using FIAPCloudGames.Api;
using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Api.Middlewares;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Authentication;
using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Request.Enderecos;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Request.Role;
using FIAPCloudGames.Domain.Dtos.Request.Usuario;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioPerfil;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioRole;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FIAPCloudGames.Presentation.Tests.Endpoints
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, "test-user"),
        new Claim(ClaimTypes.Role, "usuario"),
        new Claim(ClaimTypes.Role, "administrador")
        };

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

    }

    public class CustomWebApplicationFactory : WebApplicationFactory<ApiAssemblyReference>
    {
        public IAuthenticationAppService MockAuthenticationAppService { get; private set; } = default!;
        public IValidator<LoginRequest> MockLoginRequestValidator { get; private set; } = default!;
        public ILogger<Application.AppServices.AuthenticationAppService> MockAppServiceLogger { get; private set; } = default!;
        public IContatoAppService MockContatoAppService { get; private set; } = default!;
        public IValidator<CadastrarContatoRequest> MockCadastrarContatoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarContatoRequest> MockAtualizarContatoRequestValidator { get; private set; } = default!;
        public ILogger<Application.AppServices.ContatoAppService> MockContatoAppServiceLogger { get; private set; } = default!;
        public IEnderecoAppService MockEnderecoAppService { get; private set; } = default!;
        public IValidator<CadastrarEnderecoRequest> MockCadastrarEnderecoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarEnderecoRequest> MockAtualizarEnderecoRequestValidator { get; private set; } = default!;
        public IRoleAppService MockRoleAppService { get; private set; } = default!;
        public IValidator<CadastrarRoleRequest> MockCadastrarRoleRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarRoleRequest> MockAtualizarRoleRequestValidator { get; private set; } = default!;
        public IUsuarioGameBibliotecaAppService MockUsuarioGameBibliotecaAppService { get; private set; } = default!;
        public IValidator<ComprarGameRequest> MockComprarGameRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarBibliotecaRequest> MockAtualizarBibliotecaRequestValidator { get; private set; } = default!;
        public IGameAppService MockGameAppService { get; private set; } = default!;
        public IValidator<CadastrarGameRequest> MockCadastrarGameRequestValidator { get; private set; } = default!;
        public IValidator<ListarGamesPaginadoRequest> MockListarGamesPaginadoRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarGameRequest> MockAtualizarGameRequestValidator { get; private set; } = default!;
        public IUsuarioPerfilAppService MockUsuarioPerfilAppService { get; private set; } = default!;
        public IValidator<CadastrarUsuarioPerfilRequest> MockCadastrarUsuarioPerfilRequestValidator { get; private set; } = default!;
        public IValidator<AtualizarUsuarioPerfilRequest> MockAtualizarUsuarioPerfilRequestValidator { get; private set; } = default!;
        public IUsuarioRoleAppService MockUsuarioRoleAppService { get; private set; } = default!;
        public IValidator<ListarRolePorUsuarioRequest> MockListarRolePorUsuarioRequestValidator { get; private set; } = default!;
        public IValidator<AlterarUsuarioRoleRequest> MockAlterarUsuarioRoleRequestValidator { get; private set; } = default!;
        public IUsuarioAppService MockUsuarioAppService { get; private set; } = default!;
        public IValidator<CadastrarUsuarioRequest> MockCadastrarUsuarioRequestValidator { get; private set; } = default!;
        public IValidator<AlterarSenhaRequest> MockAlterarSenhaRequestValidator { get; private set; } = default!;

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

                var enderecoAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IEnderecoAppService));
                if (enderecoAppServiceDescriptor != null) { services.Remove(enderecoAppServiceDescriptor); }
                MockEnderecoAppService = Substitute.For<IEnderecoAppService>();
                services.AddSingleton(MockEnderecoAppService);

                var cadastrarEnderecoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarEnderecoRequest>));
                if (cadastrarEnderecoRequestValidatorDescriptor != null) { services.Remove(cadastrarEnderecoRequestValidatorDescriptor); }
                MockCadastrarEnderecoRequestValidator = Substitute.For<IValidator<CadastrarEnderecoRequest>>();
                MockCadastrarEnderecoRequestValidator.ValidateAsync(Arg.Any<CadastrarEnderecoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarEnderecoRequestValidator);

                var atualizarEnderecoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarEnderecoRequest>));
                if (atualizarEnderecoRequestValidatorDescriptor != null) { services.Remove(atualizarEnderecoRequestValidatorDescriptor); }
                MockAtualizarEnderecoRequestValidator = Substitute.For<IValidator<AtualizarEnderecoRequest>>();
                MockAtualizarEnderecoRequestValidator.ValidateAsync(Arg.Any<AtualizarEnderecoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarEnderecoRequestValidator);

                var roleAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IRoleAppService));
                if (roleAppServiceDescriptor != null) { services.Remove(roleAppServiceDescriptor); }
                MockRoleAppService = Substitute.For<IRoleAppService>();
                services.AddSingleton(MockRoleAppService);

                var cadastrarRoleRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarRoleRequest>));
                if (cadastrarRoleRequestValidatorDescriptor != null) { services.Remove(cadastrarRoleRequestValidatorDescriptor); }
                MockCadastrarRoleRequestValidator = Substitute.For<IValidator<CadastrarRoleRequest>>();
                MockCadastrarRoleRequestValidator.ValidateAsync(Arg.Any<CadastrarRoleRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarRoleRequestValidator);

                var atualizarRoleRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarRoleRequest>));
                if (atualizarRoleRequestValidatorDescriptor != null) { services.Remove(atualizarRoleRequestValidatorDescriptor); }
                MockAtualizarRoleRequestValidator = Substitute.For<IValidator<AtualizarRoleRequest>>();
                MockAtualizarRoleRequestValidator.ValidateAsync(Arg.Any<AtualizarRoleRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarRoleRequestValidator);

                var usuarioGameBibliotecaAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUsuarioGameBibliotecaAppService));
                if (usuarioGameBibliotecaAppServiceDescriptor != null) { services.Remove(usuarioGameBibliotecaAppServiceDescriptor); }
                MockUsuarioGameBibliotecaAppService = Substitute.For<IUsuarioGameBibliotecaAppService>();
                services.AddSingleton(MockUsuarioGameBibliotecaAppService);

                var comprarGameRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<ComprarGameRequest>));
                if (comprarGameRequestValidatorDescriptor != null) { services.Remove(comprarGameRequestValidatorDescriptor); }
                MockComprarGameRequestValidator = Substitute.For<IValidator<ComprarGameRequest>>();
                MockComprarGameRequestValidator.ValidateAsync(Arg.Any<ComprarGameRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockComprarGameRequestValidator);

                var atualizarBibliotecaRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarBibliotecaRequest>));
                if (atualizarBibliotecaRequestValidatorDescriptor != null) { services.Remove(atualizarBibliotecaRequestValidatorDescriptor); }
                MockAtualizarBibliotecaRequestValidator = Substitute.For<IValidator<AtualizarBibliotecaRequest>>();
                MockAtualizarBibliotecaRequestValidator.ValidateAsync(Arg.Any<AtualizarBibliotecaRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarBibliotecaRequestValidator);

                var gameAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IGameAppService));
                if (gameAppServiceDescriptor != null) { services.Remove(gameAppServiceDescriptor); }
                MockGameAppService = Substitute.For<IGameAppService>();
                services.AddSingleton(MockGameAppService);

                var cadastrarGameRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarGameRequest>));
                if (cadastrarGameRequestValidatorDescriptor != null) { services.Remove(cadastrarGameRequestValidatorDescriptor); }
                MockCadastrarGameRequestValidator = Substitute.For<IValidator<CadastrarGameRequest>>();
                MockCadastrarGameRequestValidator.ValidateAsync(Arg.Any<CadastrarGameRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarGameRequestValidator);

                var listarGamesPaginadoRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<ListarGamesPaginadoRequest>));
                if (listarGamesPaginadoRequestValidatorDescriptor != null) { services.Remove(listarGamesPaginadoRequestValidatorDescriptor); }
                MockListarGamesPaginadoRequestValidator = Substitute.For<IValidator<ListarGamesPaginadoRequest>>();
                MockListarGamesPaginadoRequestValidator.ValidateAsync(Arg.Any<ListarGamesPaginadoRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockListarGamesPaginadoRequestValidator);

                var atualizarGameRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarGameRequest>));
                if (atualizarGameRequestValidatorDescriptor != null) { services.Remove(atualizarGameRequestValidatorDescriptor); }
                MockAtualizarGameRequestValidator = Substitute.For<IValidator<AtualizarGameRequest>>();
                MockAtualizarGameRequestValidator.ValidateAsync(Arg.Any<AtualizarGameRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarGameRequestValidator);

                var usuarioPerfilAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUsuarioPerfilAppService));
                if (usuarioPerfilAppServiceDescriptor != null) { services.Remove(usuarioPerfilAppServiceDescriptor); }
                MockUsuarioPerfilAppService = Substitute.For<IUsuarioPerfilAppService>();
                services.AddSingleton(MockUsuarioPerfilAppService);

                var cadastrarUsuarioPerfilRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarUsuarioPerfilRequest>));
                if (cadastrarUsuarioPerfilRequestValidatorDescriptor != null) { services.Remove(cadastrarUsuarioPerfilRequestValidatorDescriptor); }
                MockCadastrarUsuarioPerfilRequestValidator = Substitute.For<IValidator<CadastrarUsuarioPerfilRequest>>();
                MockCadastrarUsuarioPerfilRequestValidator.ValidateAsync(Arg.Any<CadastrarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarUsuarioPerfilRequestValidator);

                var atualizarUsuarioPerfilRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AtualizarUsuarioPerfilRequest>));
                if (atualizarUsuarioPerfilRequestValidatorDescriptor != null) { services.Remove(atualizarUsuarioPerfilRequestValidatorDescriptor); }
                MockAtualizarUsuarioPerfilRequestValidator = Substitute.For<IValidator<AtualizarUsuarioPerfilRequest>>();
                MockAtualizarUsuarioPerfilRequestValidator.ValidateAsync(Arg.Any<AtualizarUsuarioPerfilRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAtualizarUsuarioPerfilRequestValidator);

                var usuarioRoleAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUsuarioRoleAppService));
                if (usuarioRoleAppServiceDescriptor != null) { services.Remove(usuarioRoleAppServiceDescriptor); }
                MockUsuarioRoleAppService = Substitute.For<IUsuarioRoleAppService>();
                services.AddSingleton(MockUsuarioRoleAppService);

                var listarRolePorUsuarioRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<ListarRolePorUsuarioRequest>));
                if (listarRolePorUsuarioRequestValidatorDescriptor != null) { services.Remove(listarRolePorUsuarioRequestValidatorDescriptor); }
                MockListarRolePorUsuarioRequestValidator = Substitute.For<IValidator<ListarRolePorUsuarioRequest>>();
                MockListarRolePorUsuarioRequestValidator.ValidateAsync(Arg.Any<ListarRolePorUsuarioRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockListarRolePorUsuarioRequestValidator);

                var alterarUsuarioRoleRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AlterarUsuarioRoleRequest>));
                if (alterarUsuarioRoleRequestValidatorDescriptor != null) { services.Remove(alterarUsuarioRoleRequestValidatorDescriptor); }
                MockAlterarUsuarioRoleRequestValidator = Substitute.For<IValidator<AlterarUsuarioRoleRequest>>();
                MockAlterarUsuarioRoleRequestValidator.ValidateAsync(Arg.Any<AlterarUsuarioRoleRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAlterarUsuarioRoleRequestValidator);

                var usuarioAppServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUsuarioAppService));
                if (usuarioAppServiceDescriptor != null) { services.Remove(usuarioAppServiceDescriptor); }
                MockUsuarioAppService = Substitute.For<IUsuarioAppService>();
                services.AddSingleton(MockUsuarioAppService);

                var cadastrarUsuarioRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<CadastrarUsuarioRequest>));
                if (cadastrarUsuarioRequestValidatorDescriptor != null) { services.Remove(cadastrarUsuarioRequestValidatorDescriptor); }
                MockCadastrarUsuarioRequestValidator = Substitute.For<IValidator<CadastrarUsuarioRequest>>();
                MockCadastrarUsuarioRequestValidator.ValidateAsync(Arg.Any<CadastrarUsuarioRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockCadastrarUsuarioRequestValidator);

                var alterarSenhaRequestValidatorDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IValidator<AlterarSenhaRequest>));
                if (alterarSenhaRequestValidatorDescriptor != null) { services.Remove(alterarSenhaRequestValidatorDescriptor); }
                MockAlterarSenhaRequestValidator = Substitute.For<IValidator<AlterarSenhaRequest>>();
                MockAlterarSenhaRequestValidator.ValidateAsync(Arg.Any<AlterarSenhaRequest>(), Arg.Any<CancellationToken>())
                    .Returns(new FluentValidation.Results.ValidationResult());
                services.AddSingleton(MockAlterarSenhaRequestValidator);
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });

                services.AddAuthorization(options => { });
            });

            builder.Configure(app =>
            {
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapRoles();
                    endpoints.MapAuthentication();
                    endpoints.MapContatos();
                    endpoints.MapEnderecos();
                    endpoints.MapUsuarioGameBiblioteca();
                    endpoints.MapGames();
                    endpoints.MapUsuarioPerfil();
                    endpoints.MapUsuarioRole();
                    endpoints.MapUsuarios();
                });
            });
        }
    }
}
