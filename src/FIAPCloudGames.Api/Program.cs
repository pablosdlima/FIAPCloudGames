using FIAPCloudGames.Api.Endpoints;
using FIAPCloudGames.Api.Extensions;
using FIAPCloudGames.Api.Middleware;
using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Domain.Dtos.Validators;
using FIAPCloudGames.IoC;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;

IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerDocumentation();
builder.Services.AddJwtAuthenticationConfig(builder.Configuration);
builder.Services.AddControllers();
builder.AddSerilogConfiguration();
builder.Services.AddValidatorsFromAssemblyContaining<CadastrarUsuarioRequestValidator>();
builder.Services.AddDbContext<Contexto>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("MS_FiapCloudGames")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDomainServices();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddAuthenticationDependencies(builder.Configuration);
builder.Host.UseSerilog();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseSerilogRequestLoggingConfiguration();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.MapContatos();
app.MapEnderecos();
app.MapGames();
app.MapRoles();
app.MapUsuarios();
app.MapUsuarioPerfil();
app.MapUsuarioGameBiblioteca();
app.MapUsuarioRole();
app.MapAuthentication();

app.Run();