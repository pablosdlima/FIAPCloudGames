using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//====================================================
public class UsuarioPerfilQuery : ObjectGraphType
{
    #region Construtor
    //-----------------------------------------------------
    public UsuarioPerfilQuery(IUsuarioPerfilAppService service)
    {
        Field<ListGraphType<UsuarioPerfilType>>("listarPerfils")
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

                    var usuarios = await service.ListarPaginacao(take, skip);

                    return usuarios;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            });

        Field<UsuarioPerfilType>("listarPerfilsPorUsuario")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<GuidGraphType>> { Name = "idUsuario" }
            ))
            .ResolveAsync(async context =>
            {
                Guid idUsuario = context.GetArgument<Guid>("idUsuario");
                return await service.BuscarPorUsuarioId(idUsuario);
            });
    }
    //-----------------------------------------------------
    #endregion
}
//====================================================
