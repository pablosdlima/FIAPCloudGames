using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//========================================================
public class EnderecoType : ObjectGraphType<EnderecoDtos>
{
    #region Construtor
    //-----------------------------------------------------
    public EnderecoType(IUsuarioAppService usuarioAppService)
    {
        Name = "Endereco";
        Description = "Enderecos do Usuário";

        Field(c => c.IdEndereco).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("Usuário a quem pertence o endereço.");
        Field(c => c.Rua).Description("Rua");
        Field(c => c.Numero).Description("Numero");
        Field(c => c.Complemento).Description("Complemento");
        Field(c => c.Bairro).Description("Bairro");
        Field(c => c.Cidade).Description("Cidade");
        Field(c => c.Estado).Description("Estado");
        Field(c => c.Cep).Description("Cep");

        Field<UsuarioType>("usuario").Description("Usuário Portador do contato")
            .Resolve(context =>
            {
                if (context.Source.UsuarioId != null) return context.Source.UsuarioId; //substituir por obj.

                var usuarioId = context.Source.UsuarioId;
                return usuarioAppService.BuscarPorId(usuarioId); //Verificar tipo de retorno...
            });
    }
    //-----------------------------------------------------
    #endregion
}
//========================================================
