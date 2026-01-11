using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//====================================================
public class UsuarioSchema : Schema
{
    public UsuarioSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<UsuarioQuery>();
    }
}
//====================================================
