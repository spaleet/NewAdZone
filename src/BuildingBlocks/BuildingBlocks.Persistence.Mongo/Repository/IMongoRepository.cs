using System.Linq.Expressions;
using BuildingBlocks.Persistence.Mongo.Base;

namespace BuildingBlocks.Persistence.Mongo.Repository;

public interface IMongoRepository<TEntity> : IDisposable
    where TEntity : MongoEntityBase
{
    Task<TEntity?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
}