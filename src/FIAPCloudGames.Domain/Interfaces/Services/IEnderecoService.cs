using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IEnderecoService : IGenericService<Endereco>
{
    List<Endereco> ListarPorUsuario(Guid usuarioId);
    Task<Endereco> Cadastrar(Endereco endereco);
    Task<(Endereco? Endereco, bool Success)> Atualizar(Endereco endereco);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}