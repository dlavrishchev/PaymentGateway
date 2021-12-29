using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PaymentGateway.Billing.Domain.Entities;

namespace PaymentGateway.Billing.Domain;

public interface IRepository<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Gets an entity with given primary key or null if not found.
    /// </summary>
    /// <param name="id">Primary key of the entity to get</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Entity or null</returns>
    Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Get a single entity by the given <paramref name="predicate"/> or null if not found.
    /// Throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">A condition to find the entity</param>
    /// <param name="cancellationToken"></param>
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Entity</returns>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Entity</returns>
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}