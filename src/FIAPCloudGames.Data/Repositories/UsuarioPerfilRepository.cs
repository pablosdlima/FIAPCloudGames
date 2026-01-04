using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Data.Repositories.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Repositories
{
    public class UsuarioPerfilRepository : GenericEntityRepository<UsuarioPerfil>, IUsuarioPerfilRepository
    {
        public UsuarioPerfilRepository(Contexto context) : base(context)
        {
        }

        public UsuarioPerfil? BuscarPorUsuarioId(Guid usuarioId)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(p => p.UsuarioId == usuarioId);
        }

        public UsuarioPerfil? BuscarPorIdEUsuario(Guid id, Guid usuarioId)
        {
            return _dbSet.FirstOrDefault(p => p.Id == id && p.UsuarioId == usuarioId);
        }
    }
}
