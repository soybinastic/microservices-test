using System.Linq.Expressions;
using Play.Common.Models;

namespace Play.Common.Repositories;

public class MongoRepository<T> : MongoDBRepository<T> where T : IEntity
{
    
    public MongoRepository(IMongoDatabase database, string collection) : base(database, collection) {}

    public override async Task<T> Get(Expression<Func<T, bool>> filter)
    {
        var entity = await (await _collection.FindAsync(filter)).FirstOrDefaultAsync();
        return entity;
    }

    public override async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter)
    {
        var entities = await (await _collection.FindAsync(filter))
            .ToListAsync();

        return entities;
    }
}