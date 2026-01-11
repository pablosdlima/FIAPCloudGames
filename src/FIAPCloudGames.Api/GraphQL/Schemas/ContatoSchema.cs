using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//====================================================
public class ContatoSchema : Schema
{
    public ContatoSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<ContatoQuery>();     
    }
}
//====================================================
