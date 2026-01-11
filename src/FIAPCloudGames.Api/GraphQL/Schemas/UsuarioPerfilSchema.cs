using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//===================================================
public class UsuarioPerfilSchema : Schema
{
    public UsuarioPerfilSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<UsuarioPerfilQuery>();
    }
}
//===================================================
