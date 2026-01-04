using FIAPCloudGames.Domain.Dtos.Request.UsuarioGameBiblioteca;
using FIAPCloudGames.Domain.Dtos.Responses.UsuarioGameBiblioteca;

namespace FIAPCloudGames.Application.Interfaces;

public interface IUsuarioGameBibliotecaAppService
{
    Task<List<BibliotecaResponse>> ListarPorUsuario(Guid usuarioId);
    Task<(BibliotecaResponse? Biblioteca, bool Success, string? ErrorMessage)> ComprarGame(ComprarGameRequest request);
    Task<(BibliotecaResponse? Biblioteca, bool Success)> Atualizar(AtualizarBibliotecaRequest request);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}