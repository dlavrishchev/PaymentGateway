using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities;

namespace Billing.Infrastructure.Data.MongoDb.Repositories;

public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly IMongoCollection<TEntity> collection;

    private string GetEntityCollectionName => typeof(TEntity).Name;

    public MongoDbRepository(IMongoDatabase database)
    {
        collection = database.GetCollection<TEntity>(GetEntityCollectionName);
    }

    public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return (await collection.FindAsync(filter, cancellationToken:cancellationToken)).SingleOrDefault(cancellationToken);
    }

    public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return (await collection.FindAsync(predicate, cancellationToken:cancellationToken)).SingleOrDefault(cancellationToken);
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await collection.InsertOneAsync(entity, cancellationToken:cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await collection.ReplaceOneAsync(filter, entity, cancellationToken:cancellationToken);
        return entity;
    }
}