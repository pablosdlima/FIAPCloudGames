using FIAPCloudGames.Domain.Interfaces.Generic;
using System.Linq.Expressions;

namespace FIAPCloudGames.Domain.Services.Generic;
public abstract class GenericServices<T> : IGenericService<T>
{
    #region Propertys
    //-----------------------------------------------------
    protected readonly IGenericEntity<T> _repository;
    //-----------------------------------------------------
    #endregion

    #region Construtor
    //-----------------------------------------------------
    protected GenericServices(IGenericEntity<T> repository) => _repository = repository;
    //-----------------------------------------------------
    #endregion

    #region Methods
    //-----------------------------------------------------
    public void Delete(T entity)
    {
        _repository.Delete(entity);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _repository.Exists(predicate);
    }

    public IQueryable<T> Get()
    {
        return _repository.Get();
    }

    public T GetById(int id)
    {
        return _repository.GetById(id);
    }

    public List<T> GetContainsId(Expression<Func<T, bool>> predicate)
    {
        return _repository.GetContainsId(predicate);
    }

    public T Insert(T entity)
    {
        _repository.Insert(entity);
        return entity;
    }

    public int LastId(Expression<Func<T, int>> predicate)
    {
        return _repository.LastId(predicate);
    }

    public T Update(T entity)
    {
        _repository.Update(entity);
        return entity;
    }
    //-----------------------------------------------------
    #endregion
}
//=========================================================
