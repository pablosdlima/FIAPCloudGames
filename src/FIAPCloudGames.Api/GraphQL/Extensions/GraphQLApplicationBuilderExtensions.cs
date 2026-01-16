using FIAPCloudGames.Api.GraphQL.Schemas;

namespace FIAPCloudGames.Api.GraphQL.Extensions;
//=====================================================
public static class GraphQLApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGraphQLSchemas(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        app.UseGraphQL<ContatoSchema>("/graphql/contatos");
        app.UseGraphQL<EnderecoSchema>("/graphql/enderecos");
        app.UseGraphQL<UsuarioGameBibliotecaSchema>("/graphql/UsuarioGameBiblioteca");
        app.UseGraphQL<UsuarioPerfilSchema>("/graphql/UsuarioPerfil");
        app.UseGraphQL<UsuarioSchema>("/graphql/Usuario");
        app.UseGraphQL<GamesSchema>("/graphql/Games");

        //if (env.IsDevelopment())
        //{
        //    app.UseGraphQLPlayground("/ui/playground");
        //}

        return app; //equivalente ao MapControllers
    }
}
//=====================================================
