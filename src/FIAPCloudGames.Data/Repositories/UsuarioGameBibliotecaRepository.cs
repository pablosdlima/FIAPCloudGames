using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Data.Repositories.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Repositories
{
    public class UsuarioGameBibliotecaRepository : GenericEntityRepository<UsuarioGameBiblioteca>, IUsuarioGameBibliotecaRepository
    {
        public UsuarioGameBibliotecaRepository(Contexto context) : base(context)
        {
        }

        public List<UsuarioGameBiblioteca> ListarPorUsuario(Guid usuarioId)
        {
            return _dbSet
                .AsNoTracking()
                .Include(b => b.Game)
                .Where(b => b.UsuarioId == usuarioId)
                .ToList();
        }

        public UsuarioGameBiblioteca? BuscarPorIdEUsuario(Guid id, Guid usuarioId)
        {
            return _dbSet
                .Include(b => b.Game)
                .FirstOrDefault(b => b.Id == id && b.UsuarioId == usuarioId);
        }

        public bool UsuarioJaPossuiGame(Guid usuarioId, Guid gameId)
        {
            return _dbSet.Any(b => b.UsuarioId == usuarioId && b.GameId == gameId);
        }
    }
}
