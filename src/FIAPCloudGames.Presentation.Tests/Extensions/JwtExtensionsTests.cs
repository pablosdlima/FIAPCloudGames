using FIAPCloudGames.Api.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FIAPCloudGames.Presentation.Tests.Extensions
{
    public class JwtExtensionsTests
    {
        [Fact]
        public void AddJwtAuthentication_DeveConfigurarCorretamenteOsServicosJwt()
        {
            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "af3b1d967c45e0df2b84ca91fe3a9d6f1148e2c0e9b7d04a51f396cb8f0a7d32"},
                {"Jwt:Issuer", "FIAPCloudGames"},
                {"Jwt:Audience", "https://api.fiapcloudgames.com"},
                {"Jwt:TokenExpirationInMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton(configuration);
            services.AddOptions();
            services.AddLogging();

            services.AddJwtAuthenticationConfig(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
            var jwtBearerOptions = optionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);

            jwtBearerOptions.Should().NotBeNull();
            jwtBearerOptions.TokenValidationParameters.Should().NotBeNull();
            jwtBearerOptions.TokenValidationParameters.ValidateIssuer.Should().BeTrue();
            jwtBearerOptions.TokenValidationParameters.ValidIssuer.Should().Be("FIAPCloudGames");
            jwtBearerOptions.TokenValidationParameters.ValidateAudience.Should().BeTrue();
            jwtBearerOptions.TokenValidationParameters.ValidAudience.Should().Be("https://api.fiapcloudgames.com");
            jwtBearerOptions.TokenValidationParameters.ValidateLifetime.Should().BeTrue();
            jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey.Should().BeTrue();

            var expectedSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("af3b1d967c45e0df2b84ca91fe3a9d6f1148e2c0e9b7d04a51f396cb8f0a7d32"));
            jwtBearerOptions.TokenValidationParameters.IssuerSigningKey.Should().BeEquivalentTo(expectedSecurityKey);
            jwtBearerOptions.TokenValidationParameters.ClockSkew.Should().Be(TimeSpan.Zero);

            var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
            authorizationService.Should().NotBeNull();
        }

        [Fact]
        public void AddJwtAuthentication_DeveLancarExcecao_QuandoKeyNaoConfigurado()
        {
            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Issuer", "FIAPCloudGames"},
                {"Jwt:Audience", "https://api.fiapcloudgames.com"},
                {"Jwt:TokenExpirationInMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton(configuration);
            services.AddOptions();
            services.AddLogging();

            Action act = () => services.AddJwtAuthenticationConfig(configuration);

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("A chave JWT (Jwt:Key) não pode ser nula ou vazia. (Parameter 'Jwt:Key')");
        }

        [Fact]
        public void AddJwtAuthentication_DeveLancarExcecao_QuandoIssuerNaoConfigurado()
        {
            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "af3b1d967c45e0df2b84ca91fe3a9d6f1148e2c0e9b7d04a51f396cb8f0a7d32"},
                {"Jwt:Audience", "https://api.fiapcloudgames.com"},
                {"Jwt:TokenExpirationInMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton(configuration);
            services.AddOptions();
            services.AddLogging();

            Action act = () => services.AddJwtAuthenticationConfig(configuration);

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("O emissor JWT (Jwt:Issuer) não pode ser nulo ou vazio. (Parameter 'Jwt:Issuer')");
        }

        [Fact]
        public void AddJwtAuthentication_DeveLancarExcecao_QuandoAudienceNaoConfigurado()
        {
            var services = new ServiceCollection();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "af3b1d967c45e0df2b84ca91fe3a9d6f1148e2c0e9b7d04a51f396cb8f0a7d32"},
                {"Jwt:Issuer", "FIAPCloudGames"},
                {"Jwt:TokenExpirationInMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton(configuration);
            services.AddOptions();
            services.AddLogging();

            Action act = () => services.AddJwtAuthenticationConfig(configuration);

            act.Should().Throw<ArgumentNullException>()
               .WithMessage("A audiência JWT (Jwt:Audience) não pode ser nula ou vazia. (Parameter 'Jwt:Audience')");
        }
    }
}
