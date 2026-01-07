using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;
using FIAPCloudGames.Domain.Services.Generic;

namespace FIAPCloudGames.Domain.Services;

public class UsuarioGameBibliotecaServices : GenericServices<UsuarioGameBiblioteca>, IUsuarioGameBibliotecaService
{
    private readonly IUsuarioGameBibliotecaRepository _bibliotecaRepository;
    private readonly IGameRepository _gameRepository;

    #region Construtor
    public UsuarioGameBibliotecaServices(
        IGenericEntityRepository<UsuarioGameBiblioteca> repository,
        IUsuarioGameBibliotecaRepository bibliotecaRepository,
        IGameRepository gameRepository) : base(repository)
    {
        _bibliotecaRepository = bibliotecaRepository;
        _gameRepository = gameRepository;
    }
    #endregion

    public List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId)
    {
        return _bibliotecaRepository.ListarPorUsuario(usuarioId);
    }

    public async Task<(UsuarioGameBiblioteca? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(UsuarioGameBiblioteca biblioteca)
    {
        var game = _gameRepository.GetById(biblioteca.GameId);

        if (game == null)
        {
            return (null, false, "Game não encontrado.");
        }

        if (_bibliotecaRepository.UsuarioJaPossuiGame(biblioteca.UsuarioId, biblioteca.GameId))
        {
            return (null, false, "Você já possui este jogo na sua biblioteca.");
        }

        biblioteca.Id = Guid.NewGuid();
        biblioteca.DataAquisicao ??= DateTimeOffset.UtcNow;

        var resultado = await _repository.Insert(biblioteca);
        return (resultado, true, null);
    }

    public async Task<(UsuarioGameBiblioteca? Biblioteca, bool Success)> Atualizar(UsuarioGameBiblioteca biblioteca)
    {
        var bibliotecaExistente = _bibliotecaRepository.BuscarPorIdEUsuario(biblioteca.Id, biblioteca.UsuarioId);

        if (bibliotecaExistente == null)
        {
            return (null, false);
        }

        bibliotecaExistente.TipoAquisicao = biblioteca.TipoAquisicao;
        bibliotecaExistente.PrecoAquisicao = biblioteca.PrecoAquisicao;
        bibliotecaExistente.DataAquisicao = biblioteca.DataAquisicao;

        var resultado = _repository.Update(bibliotecaExistente);

        return await Task.FromResult((resultado.entity, resultado.success));
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        var biblioteca = _bibliotecaRepository.BuscarPorIdEUsuario(id, usuarioId);

        if (biblioteca == null)
        {
            return false;
        }

        return await _repository.DeleteById(id);
    }
}
