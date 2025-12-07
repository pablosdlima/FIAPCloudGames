using FIAPCloudGames.Api.Extensions;
using FIAPCloudGames.IoC;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddJwtAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.ConfigureAppDependencies(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();