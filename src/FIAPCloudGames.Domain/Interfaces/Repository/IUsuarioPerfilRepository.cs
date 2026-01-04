using FIAPCloudGames.Domain.Interfaces.Generic;
using FIAPCloudGames.Domain.Models;

namespace FIAPCloudGames.Domain.Interfaces.Repository
{
    public interface IUsuarioPerfilRepository : IGenericEntityRepository<UsuarioPerfil>
    {
        UsuarioPerfil? BuscarPorUsuarioId(Guid usuarioId);
        UsuarioPerfil? BuscarPorIdEUsuario(Guid id, Guid usuarioId);
    }
}
