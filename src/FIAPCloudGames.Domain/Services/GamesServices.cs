using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class GamesServices : GenericServices<Game>, IGameService
{
    private readonly IGameRepository _gameRepository;


    public GamesServices(IGenericEntityRepository<Game> repository, IGameRepository gameRepository) : base(repository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero)
    {
        return await _gameRepository.ListarPaginado(numeroPagina, tamanhoPagina, filtro, genero);
    }
}
