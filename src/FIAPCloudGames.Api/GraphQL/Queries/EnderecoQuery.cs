using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//======================================================
public class EnderecoQuery : ObjectGraphType
{
    #region Construtor
    //-----------------------------------------------------
    public EnderecoQuery(IEnderecoAppService service)
    {
        Field<ListGraphType<EnderecoType>>("listarEnderecos")
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

                    var enderecos = await service.ListarPaginacao(take, skip);

                    return enderecos;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            });

        Field<ListGraphType<EnderecoType>>("listarEnderecosPorUsuario")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<GuidGraphType>> { Name = "idUsuario" }
            ))
            .ResolveAsync(async context =>
            {
                Guid idUsuario = context.GetArgument<Guid>("idUsuario");
                return await service.ListarPorUsuario(idUsuario);
            });
    }
    //-----------------------------------------------------
    #endregion
}
//======================================================
