using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Repository
{
    public interface IUsuarioGameBibliotecaRepository : IGenericEntityRepository<UsuarioGameBiblioteca>
    {
        List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId);
        UsuarioGameBiblioteca? BuscarPorIdEUsuario(Guid id, Guid usuarioId);
        bool UsuarioJaPossuiGame(Guid usuarioId, Guid gameId);
    }
}
