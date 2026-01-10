using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//====================================================
public class GameBibliotecaType : ObjectGraphType<UsuarioGameBibliotecaDto>
{
    #region Construtor
    //----------------------------------------------------------
    public GameBibliotecaType(IUsuarioAppService usuarioAppService)
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
                if (context.Source.UsuarioId != null) return context.Source.UsuarioId; //substituir por obj.

                var usuarioId = context.Source.UsuarioId;
                return usuarioAppService.BuscarPorId(usuarioId); //Verificar tipo de retorno...
            });
    }
    //----------------------------------------------------------
    #endregion
}
//====================================================
