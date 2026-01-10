using FIAPCloudGames.Application.AppServices;
using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using FIAPCloudGames.Domain.Models;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//===============================================
public class ContatoType : ObjectGraphType<ContatosDtos>
{
    #region Construtor
    //---------------------------------------------------------------------------------------
    public ContatoType(IUsuarioAppService usuarioAppService)
    {
        Name = "Contato";
        Description = "Contatos do Usuário";

        Field(c => c.IdContato).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("Usuário Portador do contato");
        Field(c => c.Celular).Description("Celular");
        Field(c => c.Email).Description("Email");


        Field<UsuarioType>("usuario").Description("Usuário Portador do contato")
            .Resolve(context =>
            {
                if(context.Source.UsuarioId != null) return context.Source.UsuarioId; //substituir por obj.

                var usuarioId = context.Source.UsuarioId;
                return usuarioAppService.BuscarPorId(usuarioId); //Verificar tipo de retorno...
            });
    }
    //---------------------------------------------------------------------------------------
    #endregion
}
