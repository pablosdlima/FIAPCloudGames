using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//=================================================
public class GamesSchema : Schema
{
    public GamesSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<GamesQuery>();
    }
}
//=================================================
