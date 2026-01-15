using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//=========================================================
public class UsuarioType : ObjectGraphType<UsuarioDtos>
{
    #region Construtor
    //----------------------------------------------------------
    public UsuarioType(IUsuarioAppService usuarioAppService, IDataLoaderContextAccessor dataLoader)
    {
        Name = "Usuario";
        Description = "Contatos do Usuário";

        Field(c => c.IdUsuario).Description("Chave Primária");
        Field(c => c.Nome).Description("Nome do Usuário");
        //Field(c => c.Senha).Description("Senha");
        Field(c => c.Ativo).Description("Para definir se o login é ativo.");
        Field(c => c.DataCriacao).Description("Data criação");
        Field(c => c.DataAtualizacao).Description("Data Atualização");

        Field<UsuarioType>("usuario").Description("Usuário Portador do contato")
            .Resolve(context =>
            {
                var usuarioId = context.Source.IdUsuario;

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
//=========================================================