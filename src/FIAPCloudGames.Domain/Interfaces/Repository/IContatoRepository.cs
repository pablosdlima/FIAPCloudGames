using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Repository
{
    public interface IContatoRepository : IGenericEntityRepository<Contato>
    {
        List<Contato> ListarPorUsuario(Guid usuarioId);
        Contato? BuscarPorIdEUsuario(Guid id, Guid usuarioId);
    }
}
