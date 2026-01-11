using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//====================================================
public class EnderecoSchema : Schema
{
    public EnderecoSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<EnderecoQuery>();
    }
}
//====================================================
