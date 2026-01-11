using FIAPCloudGames.Api.GraphQL.Queries;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Schemas;
//==================================================
public class GameBibliotecaSchema : Schema
{
    public GameBibliotecaSchema(IServiceProvider service) : base(service)
    {
        Query = service.GetRequiredService<GameBibliotecaQuery>();
    }
}
//==================================================
