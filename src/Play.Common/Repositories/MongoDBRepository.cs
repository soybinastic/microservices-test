using System.Linq.Expressions;
using System.Collections.ObjectModel;
using Play.Common.Models;

namespace Play.Common.Repositories;


public abstract class MongoDBRepository<T> : IRepository<T> where T : IEntity
{
    public readonly IMongoCollection<T> _collection;
    protected MongoDBRepository(IMongoDatabase database, string collection)
    {
        _collection = database.GetCollection<T>(collection);
    }

    public virtual async Task<T> Create(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task<bool> Delete(string id)
    {
        if (await GetById(id) is null)
        {
            return false;
        }

        await _collection.DeleteOneAsync(Builders<T>.Filter.Eq(i => i.Id, id));
        return true;
    }

    public virtual async Task<List<T>> GetAll()
    {
        var items = await _collection.FindAsync(_ => true);
        // Console.WriteLine($"It works from {nameof(MongoDBRepository<T>)} abstract class!!");
        return await items.ToListAsync();
    }

    public virtual async Task<T> GetById(string id)
    {
        return await _collection.Find(Builders<T>.Filter.Eq(i => i.Id, id)).FirstOrDefaultAsync();
    }

    public virtual async Task<T> Update(string id, T entity)
    {
        if (await GetById(id) is null)
        {
            throw new Exception($"Item with id of {id} not found.");
        }

        await _collection.ReplaceOneAsync(
            Builders<T>.Filter.Eq(i => i.Id, id),
            entity);

        return entity;
    }

    public abstract Task<T> Get(Expression<Func<T, bool>> filter);
    public abstract Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
}