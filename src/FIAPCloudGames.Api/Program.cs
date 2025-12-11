using FIAPCloudGames.Api.Extensions;
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

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerDocumentation();
}

app.UseSerilogRequestLoggingConfiguration();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();