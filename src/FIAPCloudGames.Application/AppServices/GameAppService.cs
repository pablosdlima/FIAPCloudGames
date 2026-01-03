using AutoMapper;
using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.Game;
using FIAPCloudGames.Domain.Dtos.Responses.Game;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class GameAppService : IGameAppService
{
    private readonly IGameService _gameService;
    private readonly IMapper _mapper;

    public GameAppService(IGameService gameService, IMapper mapper)
    {
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }


    public async Task<Game> Cadastrar(CadastrarGameRequest request)
    {
        var game = Game.Criar(request.Nome, request.Descricao, request.Genero, request.Desenvolvedor, request.Preco, request.DataRelease);
        var gameCriado = await _gameService.Insert(game);
        return gameCriado;
    }

    public CadastrarGameRequest Alterar(CadastrarGameRequest dto)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var entity = _mapper.Map<Game>(dto);
        var atualizado = _gameService.Update(entity);

        return _mapper.Map<CadastrarGameRequest>(atualizado);
    }

    public Game BuscarPorId(Guid id)
    {
        var entity = _gameService.GetById(id);

        if (entity is null)
        {
            throw new KeyNotFoundException("Game não encontrado.");
        }

        return entity;
    }

    public List<CadastrarGameRequest> Listar()
    {
        var lista = _gameService.Get();
        return _mapper.Map<List<CadastrarGameRequest>>(lista);
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
}