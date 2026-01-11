using FIAPCloudGames.Data.Data;
using FIAPCloudGames.Domain.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FIAPCloudGames.Data.Repositories.Generic;

public class GenericEntityRepository<T> : IGenericEntityRepository<T> where T : class
{
    #region Propriedades

    private readonly Contexto _context;
    protected readonly DbSet<T> _dbSet;

    #endregion

    #region Constructor

    public GenericEntityRepository(Contexto context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }


    #endregion

    #region Methods

    public void Delete(T entity)
    {
        try
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
        catch (Exception err)
        {
            Console.WriteLine(err);
        }
    }

    public async Task<bool> DeleteById(Guid id)
    {
        try
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            _context.Set<T>().Remove(entity);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception err)
        {
            throw new Exception(message: err.Message, innerException: err);
        }
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> Get()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public T GetById(Guid id)
    {
        try
        {
            return _context.Set<T>().Find(id)!;
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    public T GetByIdInt(int id)
    {
        try
        {
            return _context.Set<T>().Find(id)!;
        }
        catch (Exception err)
        {
            throw err;
        }
    }

    public List<T> GetContainsId(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public virtual async Task<T> Insert(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public int LastId(Expression<Func<T, int>> predicate)
    {
        return _context.Set<T>().Max(predicate);
    }

    public (T entity, bool success) Update(T entity)
    {
        try
        {
            _context.Update(entity);
            var result = _context.SaveChanges();
            return (entity, result > 0);
        }
        catch (Exception err)
        {
            throw new Exception(message: err.Message);
        }
    }

    public async Task<List<T>> ListarPaginacao(int take, int skip)
    {
        return await _context.Set<T>().AsNoTracking()
            .Skip(skip).Take(take).ToListAsync();
    }

    #endregion
}