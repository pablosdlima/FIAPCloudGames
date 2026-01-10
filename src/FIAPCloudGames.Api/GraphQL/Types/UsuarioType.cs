using FIAPCloudGames.Domain.Dtos;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//=========================================================
public class UsuarioType : ObjectGraphType<UsuarioDtos>
{
    #region Construtor
    //----------------------------------------------------------
    public UsuarioType()
    {
        Name = "Usuario";
        Description = "Contatos do Usuário";

        Field(c => c.IdUsuario).Description("Chave Primária");
        Field(c => c.Nome).Description("Nome do Usuário");
        //Field(c => c.Senha).Description("Senha");
        Field(c => c.Ativo).Description("Para definir se o login é ativo.");
        Field(c => c.DataCriacao).Description("Data criação");
        Field(c => c.DataAtualizacao).Description("Data Atualização");
    }
    //----------------------------------------------------------
    #endregion
}
//=========================================================