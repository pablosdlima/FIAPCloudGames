using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Domain.Interfaces.Repository;
using FIAPCloudGames.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly Contexto _context;

        public GameRepository(Contexto context)
        {
            _context = context;
        }

        public async Task<(List<Game> Jogos, int TotalRegistros)> ListarPaginado(int numeroPagina, int tamanhoPagina, string? filtro, string? genero)
        {
            var query = _context.Game.AsQueryable();

            // Filtros
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                query = query.Where(g => g.Nome.Contains(filtro) ||
                                         g.Descricao.Contains(filtro));
            }

            if (!string.IsNullOrWhiteSpace(genero))
            {
                query = query.Where(g => g.Genero == genero);
            }

            // Total de registros
            var totalRegistros = await query.CountAsync();

            // Ordenação e paginação
            var jogos = await query
                .OrderByDescending(g => g.DataRelease)
                .Skip((numeroPagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (jogos, totalRegistros);
        }
    }
}