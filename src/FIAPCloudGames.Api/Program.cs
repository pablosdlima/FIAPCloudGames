using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Api.Extensions;
using FIAPCloudGames.Api.Middleware;
using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Data.Repositories.Generic;
using FIAPCloudGames.Domain.Dtos.Validators;
using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Services;
using FIAPCloudGames.IoC;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

#region Swagger

builder.Services.AddSwaggerDocumentation();

#endregion

#region Segurança

builder.Services.AddJwtAuthenticationConfig(builder.Configuration);

#endregion

#region Controllers
//-------------------------------------------------------
builder.Services.AddControllers();
//-------------------------------------------------------
#endregion

#region Serilog
//-------------------------------------------------------
builder.AddSerilogConfiguration();
//-------------------------------------------------------
#endregion

#region Validators

builder.Services.AddValidatorsFromAssemblyContaining<CadastrarUsuarioRequestValidator>();

#endregion

#region Contexto Base de dados

builder.Services.AddDbContext<Contexto>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("MS_FiapCloudGames")));

#endregion

#region Mappers
//-------------------------------------------------------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//-------------------------------------------------------
#endregion

#region IoC / Injeção de Dependência
//--------------------------------------------------------------

// Domain Services
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
builder.Services.AddScoped<IGameService, GamesServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IUsuarioService, UsuarioServices>();
builder.Services.AddScoped<IUsuarioGameService, UsuarioGameServices>();
builder.Services.AddScoped<IUsuarioPerfilService, UsuarioPerfilServices>();
builder.Services.AddScoped<IUsuarioRoleServices, UsuarioRoleServices>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationServices>();

// Repositório Genérico
builder.Services.AddScoped(typeof(IGenericEntityRepository<>), typeof(GenericEntityRepository<>));

// Application Services
builder.Services.AddScoped<IContatoAppService, ContatoAppService>();
builder.Services.AddScoped<IEnderecoAppService, EnderecoAppService>();
builder.Services.AddScoped<IGameAppService, GameAppService>();
builder.Services.AddScoped<IRoleAppService, RoleAppService>();
builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();
builder.Services.AddScoped<IUsuarioGameAppService, UsuarioGameAppService>();
builder.Services.AddScoped<IUsuarioPerfilAppService, UsuarioPerfilAppService>();
builder.Services.AddScoped<IUsuarioRoleAppService, UsuarioRoleAppService>();

// IoC centralizado (se existir)
builder.Services.ConfigureAppDependencies(builder.Configuration);

//--------------------------------------------------------------
#endregion

// Substituir o logger padrão pelo Serilog
builder.Host.UseSerilog();

var app = builder.Build();

#region Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation(); // Usar sua extension!
}
#endregion

#region Pipeline HTTP
//-------------------------------------------------------
app.UseSerilogRequestLoggingConfiguration();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add middleware de tratamento de erros
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
//-------------------------------------------------------
#endregion

#region Endpoints (Minimal APIs)

//app.MapContatos();
//app.MapEnderecos();
//app.MapGames();
//app.MapRoles();
app.MapUsuarios();
//app.MapUsuariosPerfil();
//app.MapUsuarioGames();
//app.MapUsuarioRole();
app.MapAuthentication();

#endregion

app.Run();
