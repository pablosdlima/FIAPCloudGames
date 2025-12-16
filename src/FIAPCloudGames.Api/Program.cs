using FIAPCloudGames.Api.Extensions;
using FIAPCloudGames.Api.Middleware;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Services;
using FIAPCloudGames.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddJwtAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.ConfigureAppDependencies(builder.Configuration);
builder.AddSerilogConfiguration();

// Substituir o logger padrão pelo Serilog
builder.Host.UseSerilog();

#region I.J
//--------------------------------------------------------------
builder.Services.AddScoped<IContatoService, ContatoService>();
//--------------------------------------------------------------
builder.Services.AddScoped<IEnderecoService, EnderecoService>();
//--------------------------------------------------------------
builder.Services.AddScoped<IGameService, GamesServices>();
//--------------------------------------------------------------
builder.Services.AddScoped<IRoleServices, RoleServices>();
//--------------------------------------------------------------
builder.Services.AddScoped<IUsuarioService, UsuarioServices>();
//--------------------------------------------------------------
builder.Services.AddScoped<IUsuarioGameService, UsuarioGameServices>();
//--------------------------------------------------------------
builder.Services.AddScoped<IUsuarioPerfilService, UsuarioPerfilServices>();
//--------------------------------------------------------------
builder.Services.AddScoped<IUsuarioRoleServices, UsuarioRoleServices>();
//--------------------------------------------------------------
#endregion

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerDocumentation();
}

app.UseSerilogRequestLoggingConfiguration();

// Add middleware de tratamento de erros
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();