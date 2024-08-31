using Api.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Api.Shared.Persistence.Contexts;

public class EfRepository<TEntity, TEntityId> : IRepository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
{
    protected readonly BaseDbContext Context;

    public EfRepository(BaseDbContext context)
    {
        Context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        entity.DeletedAt = DateTime.Now;
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Context.Set<TEntity>();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Context.Set<TEntity>();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToListAsync(cancellationToken);
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
    {
        entity.UpdatedAt = DateTime.Now;
        EntityEntry<TEntity> attachedEntity = Context.Attach(entity);
        foreach (var property in updatedProperties)
        {
            attachedEntity.Property(property).IsModified = true;
        }
        if (updatedProperties.Length > 0)
            await Context.SaveChangesAsync();
        return entity;
    }
}
