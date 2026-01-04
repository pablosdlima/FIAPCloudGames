using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;
public interface IGameService : IGenericServices<Game>
{
    Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero);

    Task<(Game? Game, bool Success)> AtualizarGame(Game game);
}