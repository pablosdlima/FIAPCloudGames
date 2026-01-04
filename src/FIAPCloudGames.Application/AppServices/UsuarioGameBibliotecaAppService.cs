using FIAPCloudGames.Application.Interfaces;
using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Interfaces.Services;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Application.AppServices;

public class UsuarioGameBibliotecaAppService : IUsuarioGameBibliotecaAppService
{
    private readonly IUsuarioGameBibliotecaService _bibliotecaService;

    public UsuarioGameBibliotecaAppService(IUsuarioGameBibliotecaService bibliotecaService)
    {
        _bibliotecaService = bibliotecaService;
    }

    public async Task<List<BibliotecaResponse>> ListarPorUsuario(Guid usuarioId)
    {
        var biblioteca = _bibliotecaService.ListarPorUsuario(usuarioId);

        var bibliotecaResponse = biblioteca.Select(b => new BibliotecaResponse
        {
            Id = b.Id,
            UsuarioId = b.UsuarioId,
            GameId = b.GameId,
            TipoAquisicao = b.TipoAquisicao,
            PrecoAquisicao = b.PrecoAquisicao,
            DataAquisicao = b.DataAquisicao,
            NomeGame = b.Game?.Nome,
            GeneroGame = b.Game?.Genero
        }).ToList();

        return await Task.FromResult(bibliotecaResponse);
    }

    public async Task<(BibliotecaResponse? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(ComprarGameRequest request)
    {
        var biblioteca = new UsuarioGameBiblioteca
        {
            UsuarioId = request.UsuarioId,
            GameId = request.GameId,
            TipoAquisicao = request.TipoAquisicao,
            PrecoAquisicao = request.PrecoAquisicao,
            DataAquisicao = request.DataAquisicao
        };

        var (bibliotecaComprada, sucesso, errorMessage) = await _bibliotecaService.ComprarGame(biblioteca);

        if (!sucesso || bibliotecaComprada == null)
        {
            return (null, false, errorMessage);
        }

        var response = new BibliotecaResponse
        {
            Id = bibliotecaComprada.Id,
            UsuarioId = bibliotecaComprada.UsuarioId,
            GameId = bibliotecaComprada.GameId,
            TipoAquisicao = bibliotecaComprada.TipoAquisicao,
            PrecoAquisicao = bibliotecaComprada.PrecoAquisicao,
            DataAquisicao = bibliotecaComprada.DataAquisicao,
            NomeGame = bibliotecaComprada.Game?.Nome,
            GeneroGame = bibliotecaComprada.Game?.Genero
        };

        return (response, true, null);
    }

    public async Task<(BibliotecaResponse? Biblioteca, bool Success)> Atualizar(AtualizarBibliotecaRequest request)
    {
        var biblioteca = new UsuarioGameBiblioteca
        {
            Id = request.Id,
            UsuarioId = request.UsuarioId,
            GameId = request.GameId,
            TipoAquisicao = request.TipoAquisicao,
            PrecoAquisicao = request.PrecoAquisicao,
            DataAquisicao = request.DataAquisicao
        };

        var (bibliotecaAtualizada, sucesso) = await _bibliotecaService.Atualizar(biblioteca);

        if (!sucesso || bibliotecaAtualizada == null)
        {
            return (null, false);
        }

        var response = new BibliotecaResponse
        {
            Id = bibliotecaAtualizada.Id,
            UsuarioId = bibliotecaAtualizada.UsuarioId,
            GameId = bibliotecaAtualizada.GameId,
            TipoAquisicao = bibliotecaAtualizada.TipoAquisicao,
            PrecoAquisicao = bibliotecaAtualizada.PrecoAquisicao,
            DataAquisicao = bibliotecaAtualizada.DataAquisicao,
            NomeGame = bibliotecaAtualizada.Game?.Nome,
            GeneroGame = bibliotecaAtualizada.Game?.Genero
        };

        return (response, true);
    }

    public async Task<bool> Deletar(Guid id, Guid usuarioId)
    {
        return await _bibliotecaService.Deletar(id, usuarioId);
    }
}
