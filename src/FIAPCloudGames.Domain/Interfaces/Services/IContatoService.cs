using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Services;

public interface IContatoService : IGenericServices<Contato>
{
    List<Contato> ListarPorUsuario(Guid usuarioId);
    Task<Contato> Cadastrar(Contato contato);
    Task<(Contato? Contato, bool Success)> Atualizar(Contato contato);
    Task<bool> Deletar(Guid id, Guid usuarioId);
}