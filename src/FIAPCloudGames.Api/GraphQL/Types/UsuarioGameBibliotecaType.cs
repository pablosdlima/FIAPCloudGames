using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//====================================================
public class UsuarioGameBibliotecaType : ObjectGraphType<UsuarioGameBibliotecaDto>
{
    #region Construtor
    //----------------------------------------------------------
    public UsuarioGameBibliotecaType(IUsuarioAppService usuarioAppService, IDataLoaderContextAccessor dataLoader)
    {
        Name = "GameBiblioteca";
        Description = "GameBiblioteca";

        Field(c => c.IdUsuarioGame).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("entidade do Usuário");
        Field(c => c.GameId).Description("entidade do Game");
        Field(c => c.TipoAquisicao).Description("Tipo");
        Field(c => c.PrecoAquisicao).Description("Preço Aquisição");
        Field(c => c.DataAquisicao).Description("Data Aquisição");

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
