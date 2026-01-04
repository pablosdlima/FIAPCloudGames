using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Repository
{
    public interface IGameRepository : IGenericEntityRepository<Game>
    {
        Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero);
    }
}
