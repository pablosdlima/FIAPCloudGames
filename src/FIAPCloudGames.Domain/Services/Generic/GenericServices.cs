using FIAPCloudGames.Domain.Interfaces.Generic;
using System.Linq.Expressions;

namespace FIAPCloudGames.Domain.Services.Generic;
public abstract class GenericServices<T> : IGenericService<T>
{
    #region Propertys

    protected readonly IGenericEntityRepository<T> _repository;

    #endregion

    #region Construtor

    protected GenericServices(IGenericEntityRepository<T> repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public void Delete(T entity)
    {
        _repository.Delete(entity);
    }

    public async Task<bool> DeleteById(Guid id)
    {
        return await _repository.DeleteById(id);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
        return _repository.Exists(predicate);
    }

    public IQueryable<T> Get()
    {
        return _repository.Get();
    }

    public T GetById(Guid id)
    {
        return _repository.GetById(id);
    }

    public List<T> GetContainsId(Expression<Func<T, bool>> predicate)
    {
        return _repository.GetContainsId(predicate);
    }

    public async Task<T> Insert(T entity)
    {
        await _repository.Insert(entity);
        return entity;
    }

    public int LastId(Expression<Func<T, int>> predicate)
    {
        return _repository.LastId(predicate);
    }

    public async Task<(T entity, bool success)> Update(T entity)
    {
        var (updatedEntity, success) = _repository.Update(entity);
        return (updatedEntity, success);
    }

    #endregion
}