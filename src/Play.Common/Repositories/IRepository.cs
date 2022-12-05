using Play.Common.Models;

namespace Play.Common.Repositories;


public interface IRepository<T> where T : IEntity
{
    Task<T> Create(T entity);
    Task<bool> Delete(string id);
    Task<List<T>> GetAll();
    Task<T> GetById(string id);
    Task<T> Update(string id, T entity);
}