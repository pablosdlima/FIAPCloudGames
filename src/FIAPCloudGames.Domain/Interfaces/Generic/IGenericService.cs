using System.Linq.Expressions;

namespace FIAPCloudGames.Domain.Interfaces.Generic;
//=====================================================
public interface IGenericService<T>
{
    bool Exists(Expression<Func<T, bool>> predicate);
    void Delete(T entity);
    T Insert(T entity);
    T Update(T entity);
    IQueryable<T> Get();
    T GetById(int id);
    List<T> GetContainsId(Expression<Func<T, bool>> predicate);
    int LastId(Expression<Func<T, int>> predicate);
}
//=====================================================