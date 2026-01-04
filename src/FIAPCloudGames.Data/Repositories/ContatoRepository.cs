using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Data.Repositories.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Repositories
{
    public class ContatoRepository : GenericEntityRepository<Contato>, IContatoRepository
    {
        public ContatoRepository(Contexto context) : base(context)
        {
        }

        public List<Contato> ListarPorUsuario(Guid usuarioId)
        {
            return _dbSet
                .AsNoTracking()
                .Where(c => c.UsuarioId == usuarioId)
                .ToList();
        }

        public Contato? BuscarPorIdEUsuario(Guid id, Guid usuarioId)
        {
            return _dbSet
                .FirstOrDefault(c => c.Id == id && c.UsuarioId == usuarioId);
        }
    }
}