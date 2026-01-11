using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//========================================================================
public class GameBibliotecaQuery : ObjectGraphType
{
    #region Construtor
    //--------------------------------------------------------------------
    public GameBibliotecaQuery(IGameAppService service)
    {
        Field<ListGraphType<GameBibliotecaType>>("listarGames")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" }
            ))
            .ResolveAsync(async context =>
            {
                try
                {
                    int take = context.GetArgument<int>("take");
                    int skip = context.GetArgument<int>("skip");

                    var games = await service.ListarPaginacao(take, skip);

                    return games;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            });
    }
    //--------------------------------------------------------------------
    #endregion
}
//========================================================================