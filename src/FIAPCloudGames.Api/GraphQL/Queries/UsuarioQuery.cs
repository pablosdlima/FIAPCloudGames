using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//===========================================================
public class UsuarioQuery : ObjectGraphType
{
    #region Construtor
    //-------------------------------------------------------
    public UsuarioQuery(IUsuarioAppService service)
    {
        Field<UsuarioType>("obterUsuario")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<GuidGraphType>> { Name = "idUsuario" }
            ))
            .ResolveAsync(async context =>
            {
                Guid idUsuario = context.GetArgument<Guid>("idUsuario");
                return service.BuscarPorId(idUsuario);
            });
    }
    //-------------------------------------------------------
    #endregion
}
//===========================================================
