using FIAPCloudGames.Domain.Dtos.Request.Enderecos;

namespace FIAPCloudGames.Application.Interfaces;

public interface IEnderecoAppService
{
    Task<List<EnderecoResponse>> ListarPorUsuario(Guid usuarioId);
    Task<EnderecoResponse> Cadastrar(CadastrarEnderecoRequest request);
    Task<(EnderecoResponse? Endereco, bool Success)> Atualizar(AtualizarEnderecoRequest request);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}