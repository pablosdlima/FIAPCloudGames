using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;
using FIAPCloudGames.Domain.Models;
using GraphQL;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Queries;
//=========================================================
public class ContatoQuery : ObjectGraphType
{
    #region Construtor
    //-----------------------------------------------------
    public ContatoQuery(IContatoAppService service)
    {
        Field<ListGraphType<ContatoType>>("listarContatos")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip"}
            ))
            .ResolveAsync(async context =>
            {
                try
                {
                    int take = context.GetArgument<int>("take");
                    int skip = context.GetArgument<int>("skip");

                    var contatos = await service.ListarPaginacao(take, skip);

                    List<ContatosDtos> contatosDto = contatos.Select(c => new ContatosDtos
                    {
                        IdContato = c.Id,
                        UsuarioId = c.UsuarioId,
                        Celular = c.Celular,
                        Email = c.Email
                    }).ToList();

                    return contatosDto;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                }
            });

        Field<ListGraphType<ContatoType>>("listarContatosPorUsuario")
            .Arguments(new QueryArguments(
                new QueryArgument<NonNullGraphType<GuidGraphType>> { Name = "idUsuario" }
            ))
            .ResolveAsync(async context =>
            {
                Guid idUsuario = context.GetArgument<Guid>("idUsuario");
                var contatos = await service.ListarPorUsuario(idUsuario);

                List<ContatosDtos> contatosDto = contatos.Select(c => new ContatosDtos
                {
                    IdContato = c.Id,
                    UsuarioId = c.UsuarioId,
                    Celular = c.Celular,
                    Email = c.Email
                }).ToList();

                return contatosDto;
            });
    }
    //-----------------------------------------------------
    #endregion
}
//=========================================================
