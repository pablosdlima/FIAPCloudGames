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

    public async Task<(Game? Game, bool Success)> AtualizarGame(Game game)
    {
        var gameExistente = _repository.GetById(game.Id);

        if (gameExistente == null)
        {
            return (null, false);
        }

        gameExistente.Nome = game.Nome;
        gameExistente.Descricao = game.Descricao;
        gameExistente.Genero = game.Genero;
        gameExistente.Desenvolvedor = game.Desenvolvedor;
        gameExistente.Preco = game.Preco;
        gameExistente.DataRelease = game.DataRelease;

        var resultado = _repository.Update(gameExistente);
        return (resultado.entity, resultado.success);
    }
}