using FIAPCloudGames.Api.GraphQL.Types;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Models;
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
                    List<UsuarioPerfilDto> dtos = usuarios.Select(c => new UsuarioPerfilDto()
                    {
                        IdPerfil = c.Id,
                        UsuarioId = c.UsuarioId,
                        NomeCompleto = c.NomeCompleto,
                        DataNascimento = c.DataNascimento
                    }).ToList();

                    return dtos;
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
                var usuarioPerfil = await service.BuscarPorUsuarioId(idUsuario);

                UsuarioPerfilDto dto = new()
                {
                    IdPerfil = usuarioPerfil.Id,
                    UsuarioId = usuarioPerfil.UsuarioId,
                    NomeCompleto = usuarioPerfil.NomeCompleto,
                    DataNascimento = usuarioPerfil.DataNascimento
                };
                return dto;
            });
    }
    //-----------------------------------------------------
    #endregion
}
//====================================================
