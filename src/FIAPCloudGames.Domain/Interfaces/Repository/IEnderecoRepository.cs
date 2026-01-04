using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Repository
{
    public interface IEnderecoRepository : IGenericEntityRepository<Endereco>
    {
        List<Endereco> ListarPorUsuario(Guid usuarioId);
        Endereco? BuscarPorIdEUsuario(Guid id, Guid usuarioId);
    }
}
