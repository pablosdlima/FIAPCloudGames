using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//====================================================
public class UsuarioPerfilType : ObjectGraphType<UsuarioPerfilDto>
{
    #region Construtor
    //----------------------------------------------------------
    public UsuarioPerfilType(IUsuarioAppService usuarioAppService)
    {
        Name = "UsuarioPerfil";
        Description = "Perfil do Usuário";

        Field(c => c.IdPerfil).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("Usuário");
        Field(c => c.GameId).Description("GameId");
        Field(c => c.TipoAquisicao).Description("TipoAquisicao");
        Field(c => c.PrecoAquisicao).Description("PrecoAquisicao");
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
