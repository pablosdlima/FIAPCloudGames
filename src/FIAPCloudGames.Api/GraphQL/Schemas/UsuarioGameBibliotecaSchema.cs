using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//==================================================
public class UsuarioGameBibliotecaSchema : Schema
{
    public UsuarioGameBibliotecaSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<UsuarioGameBibliotecaQuery>();
    }
}
//==================================================
