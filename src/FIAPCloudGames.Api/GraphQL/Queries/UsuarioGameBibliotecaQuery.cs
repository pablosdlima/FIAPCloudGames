using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//========================================================================
public class UsuarioGameBibliotecaQuery : ObjectGraphType
{
    #region Construtor
    //--------------------------------------------------------------------
    public UsuarioGameBibliotecaQuery(IUsuarioGameBibliotecaAppService service)
    {
        Field<ListGraphType<UsuarioGameBibliotecaType>>("listarGames")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" }
            ))
            .ResolveAsync(async context =>
            {
                //try
                //{
                    //    int take = context.GetArgument<int>("take");
                    //    int skip = context.GetArgument<int>("skip");

                    //    var usuarioGame = await service.ListarPaginacao(take, skip);

                    //    List<UsuarioGameBibliotecaDto> contatosDto = usuarioGame.Select(c => new EnderecoDtos
                    //    {
                    //        IdEndereco = c.Id,
                    //        UsuarioId = c.UsuarioId,
                    //        Rua = c.Rua,
                    //        Numero = c.Numero,
                    //        Complemento = c.Complemento,
                    //        Bairro = c.Bairro,
                    //        Estado = c.Estado,
                    //        Cidade = c.Cidade,
                    //        Cep = c.Cep

                    //    }).ToList();

                    //    return contatosDto;
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw new Exception($"{ex.Message}");
                    //}
                    return null;
            });
    }
    //--------------------------------------------------------------------
    #endregion
}
//========================================================================