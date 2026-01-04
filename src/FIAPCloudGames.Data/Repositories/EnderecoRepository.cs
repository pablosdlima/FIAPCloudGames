using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Data.Repositories.Generic;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Repositories
{
    public class EnderecoRepository : GenericEntityRepository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(Contexto context) : base(context)
        {
        }

        public List<Endereco> ListarPorUsuario(Guid usuarioId)
        {
            return _dbSet
                .AsNoTracking()
                .Where(e => e.UsuarioId == usuarioId)
                .ToList();
        }

        public Endereco? BuscarPorIdEUsuario(Guid id, Guid usuarioId)
        {
            return _dbSet
                .FirstOrDefault(e => e.Id == id && e.UsuarioId == usuarioId);
        }
    }
}
