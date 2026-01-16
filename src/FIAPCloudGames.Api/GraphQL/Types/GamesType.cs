using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace FIAPCloudGames.Api.GraphQL.Types;
//===================================================
public class GamesType : ObjectGraphType<GameDtos>
{
    #region Construtor
    public GamesType(IUsuarioAppService usuarioAppService,
        IDataLoaderContextAccessor dataLoader)
    {
        Name = "Game";
        Description = "Jogos cadastrados na plataforma";

        Field(j => j.IdGame)
            .Description("Chave Primária do jogo");

        Field(j => j.Nome)
            .Description("Nome do jogo");

        Field(j => j.Descricao)
            .Description("Descrição do jogo");

        Field(j => j.Genero)
            .Description("Gênero do jogo");

        Field(j => j.Desenvolvedor)
            .Description("Desenvolvedor do jogo");

        Field(j => j.Preco)
            .Description("Preço do jogo");

        Field(j => j.DataCriacao, nullable: true)
            .Description("Data de criação do registro");

        Field(j => j.DataRelease, nullable: true)
            .Description("Data de lançamento do jogo");

    }
    #endregion
}
//===================================================
