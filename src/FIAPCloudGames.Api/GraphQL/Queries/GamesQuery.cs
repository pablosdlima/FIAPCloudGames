using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//==============================================
public class GamesQuery : ObjectGraphType
{
    #region Construtor
    //-----------------------------------------------------
    public GamesQuery(IGameAppService service)
    {
        Field<ListGraphType<GamesType>>("listarGames")
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

                   List<GameDtos> gamesDto = games.Select(c => new GameDtos
                   {
                       IdGame = c.Id,
                       Nome = c.Nome,
                       Descricao = c.Descricao,
                       Genero = c.Genero,
                       Preco = c.Preco,
                       DataRelease = c.DataRelease
                   }).ToList();

                   return gamesDto;
               }
               catch (Exception ex)
               {
                   throw new Exception($"{ex.Message}");
               }
           });
    }
    //-----------------------------------------------------
    #endregion
}
//==============================================
