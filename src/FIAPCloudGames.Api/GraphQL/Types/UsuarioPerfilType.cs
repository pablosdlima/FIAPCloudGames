using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//====================================================
public class UsuarioPerfilType : ObjectGraphType<UsuarioPerfilDto>
{
    #region Construtor
    //----------------------------------------------------------
    public UsuarioPerfilType(IUsuarioAppService usuarioAppService, IDataLoaderContextAccessor dataLoader)
    {
        Name = "UsuarioPerfil";
        Description = "Perfil do Usuário";

        Field(c => c.IdPerfil).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("Usuário");
        Field(c => c.GameId).Description("GameId");
        Field(c => c.NomeCompleto).Description("NomeCompleto");
        Field(c => c.PrecoAquisicao).Description("PrecoAquisicao");
        Field(c => c.DataNascimento).Description("DataNascimento");

        Field<UsuarioType>("usuario").Description("Usuário Portador do contato")
            .Resolve(context =>
            {
                var usuarioId = context.Source.UsuarioId;

                if (usuarioId == Guid.Empty)
                    return null;

                var loader = dataLoader.Context
                    .GetOrAddBatchLoader<Guid, UsuarioDtos>(
                        "UsuariosPorId",
                        async ids =>
                        {
                            var usuarios = await usuarioAppService.BuscarPorIdsAsync(ids);
                            return usuarios;
                        });

                return loader.LoadAsync(usuarioId);
            });
    }
    //----------------------------------------------------------
    #endregion
}
//====================================================
