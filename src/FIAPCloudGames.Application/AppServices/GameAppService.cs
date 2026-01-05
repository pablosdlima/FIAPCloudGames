using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;
using FIAPCloudGames.Domain.Exceptions;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class GameAppService : IGameAppService
{
    private readonly IGameService _gameService;

    public GameAppService(IGameService gameService)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
    }


    public async Task<Game> Cadastrar(CadastrarGameRequest request)
    {
        var game = Game.Criar(request.Nome, request.Descricao, request.Genero, request.Desenvolvedor, request.Preco, request.DataRelease);
        var gameCriado = await _gameService.Insert(game);
        return gameCriado;
    }

    public Game BuscarPorId(Guid id)
    {
        var entity = _gameService.GetById(id);

        if (entity is null)
        {
            throw new NotFoundException("Game não encontrado.");
        }

        return entity;
    }

    public async Task<ListarGamesPaginadoResponse> ListarGamesPaginado(ListarGamesPaginadoRequest request)
    {
        var (jogos, totalRegistros) = await _gameService.ListarPaginado(request.NumeroPagina, request.TamanhoPagina, request.Filtro, request.Genero);

        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)request.TamanhoPagina);

        var jogosResponse = jogos.Select(g => new GameResponse
        {
            Id = g.Id,
            Nome = g.Nome,
            Descricao = g.Descricao,
            Genero = g.Genero,
            Desenvolvedor = g.Desenvolvedor,
            Preco = g.Preco,
            DataRelease = g.DataRelease
        }).ToList();

        return new ListarGamesPaginadoResponse
        {
            PaginaAtual = request.NumeroPagina,
            TamanhoPagina = request.TamanhoPagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros,
            TemPaginaAnterior = request.NumeroPagina > 1,
            TemProximaPagina = request.NumeroPagina < totalPaginas,
            Jogos = jogosResponse
        };
    }

    public async Task<(AtualizarGameResponse? Game, bool Success)> AtualizarGame(AtualizarGameRequest request)
    {
        var game = new Game
        {
            Id = request.Id,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Genero = request.Genero,
            Desenvolvedor = request.Desenvolvedor,
            Preco = request.Preco,
            DataRelease = request.DataRelease.HasValue
            ? new DateTimeOffset(request.DataRelease.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)
            : null
        };

        var (gameAtualizado, sucesso) = await _gameService.AtualizarGame(game);

        if (!sucesso || gameAtualizado == null)
        {
            return (null, false);
        }

        var response = new AtualizarGameResponse
        {
            Id = gameAtualizado.Id,
            Nome = gameAtualizado.Nome,
            Descricao = gameAtualizado.Descricao,
            Genero = gameAtualizado.Genero,
            Desenvolvedor = gameAtualizado.Desenvolvedor,
            Preco = gameAtualizado.Preco,
            DataCriacao = gameAtualizado.DataCriacao,
            DataRelease = gameAtualizado.DataRelease
        };

        return (response, true);
    }
}