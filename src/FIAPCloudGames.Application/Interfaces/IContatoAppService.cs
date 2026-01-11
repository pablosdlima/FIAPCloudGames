using FIAPCloudGames.Domain.Dtos.Request.Contato;
using FIAPCloudGames.Domain.Dtos.Responses.Contato;

namespace FIAPCloudGames.Application.Interfaces;

public interface IContatoAppService
{
    Task<List<ContatoResponse>> ListarPorUsuario(Guid usuarioId);
    Task<List<ContatoResponse>> ListarPaginacao(int take, int skip);
    Task<ContatoResponse> Cadastrar(CadastrarContatoRequest request);
    Task<(ContatoResponse? Contato, bool Success)> Atualizar(AtualizarContatoRequest request);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}