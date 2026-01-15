using FIAPCloudGames.Api.GraphQL.Queries;
using FIAPCloudGames.Api.GraphQL.Schemas;
using System.Diagnostics.CodeAnalysis;

namespace FIAPCloudGames.Api.GraphQL.Extensions;
//==========================================================

//não consegui aplicar a ref de presentation em crossCutting.

[ExcludeFromCodeCoverage]
public static class GraphQLServiceCollectionExtensions
{
    public static IServiceCollection AddGraphQLDependencies(this IServiceCollection services)
    {
        #region Querys
        //------------------------------------------------
        services.AddScoped<ContatoQuery>();
        services.AddScoped<EnderecoQuery>();
        services.AddScoped<UsuarioGameBibliotecaQuery>();
        services.AddScoped<UsuarioPerfilQuery>();
        services.AddScoped<UsuarioQuery>();
        //------------------------------------------------
        #endregion

        #region Schemas
        //------------------------------------------------
        services.AddScoped<ContatoSchema>();
        //------------------------------------------------
        services.AddScoped<EnderecoSchema>();
        //------------------------------------------------
        services.AddScoped<UsuarioGameBibliotecaSchema>();
        //------------------------------------------------
        services.AddScoped<UsuarioPerfilSchema>();
        //------------------------------------------------
        services.AddScoped<UsuarioSchema>();
        //------------------------------------------------
        #endregion

        return services;
    }
}
//==========================================================
