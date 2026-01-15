using FIAPCloudGames.Application.Dtos;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;

public class ContatoType : ObjectGraphType<ContatosDtos>
{
    public ContatoType(
        IUsuarioAppService usuarioAppService,
        IDataLoaderContextAccessor dataLoader)
    {
        Name = "Contato";
        Description = "Contatos do Usuário";

        Field(c => c.IdContato).Description("Chave Primária");
        Field(c => c.UsuarioId).Description("Usuário Portador do contato");
        Field(c => c.Celular).Description("Celular");
        Field(c => c.Email).Description("Email");

        Field<UsuarioType>("usuario")
            .Description("Usuário Portador do contato")
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
}
