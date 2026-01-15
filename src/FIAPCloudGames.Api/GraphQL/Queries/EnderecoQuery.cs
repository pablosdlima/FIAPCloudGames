using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Dtos;
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

                    List<EnderecoDtos> dtos = enderecos.Select(c => new EnderecoDtos
                    {
                        IdEndereco = c.Id,
                        UsuarioId = c.UsuarioId,
                        Rua = c.Rua,
                        Numero = c.Numero,
                        Complemento = c.Complemento,
                        Bairro = c.Bairro,
                        Estado = c.Estado,
                        Cidade = c.Cidade,
                        Cep = c.Cep

                    }).ToList();

                    return dtos;
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
                var enderecos = await service.ListarPorUsuario(idUsuario);

                List<EnderecoDtos> enderecosDtos = enderecos.Select(c => new EnderecoDtos
                {
                    IdEndereco = c.Id,
                    UsuarioId = c.UsuarioId,
                    Rua = c.Rua,
                    Numero = c.Numero,
                    Complemento = c.Complemento,
                    Bairro = c.Bairro,
                    Estado = c.Estado,
                    Cidade = c.Cidade,
                    Cep = c.Cep

                }).ToList();
                return enderecosDtos;
            });
    }
    //-----------------------------------------------------
    #endregion
}
//======================================================
