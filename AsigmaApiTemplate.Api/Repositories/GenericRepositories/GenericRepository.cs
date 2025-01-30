using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Data;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AsigmaApiTemplate.Api.Repositories.GenericRepositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : BaseModel
{
    private readonly DbSet<T> _entities = context.Set<T>();

    public async Task<(List<T> data, double totalCount)> SearchAsync(int page, int pageSize,
        Expression<Func<T, bool>>? filters, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        var query = _entities
            .Where(q => !q.IsDeleted);

        if (filters != null)
        {
            query = query.Where(filters);
        }

        if (includes != null)
        {
            query = includes(query);
        }

        var totalCount = await query.CountAsync();
        var result = await query
            .OrderByDescending(q => q.DateCreated)
            .ThenBy(q => q.DateUpdated).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (result, totalCount);
    }

    public async Task<List<T>> SearchAsync(Expression<Func<T, bool>>[]? filters,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        var query = _entities
            .Where(q => !q.IsDeleted);

        if (filters != null && filters.Any())
        {
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        }

        if (includes != null)
        {
            query = includes(query);
        }

        var result = await query
            .OrderByDescending(q => q.DateCreated)
            .ThenBy(q => q.DateUpdated).ToListAsync();

        return result;
    }

    public async Task<PaginatedList<T>> GetAllAsync()
    {
        IQueryable<T> query = _entities;

        var totalCount = await query.CountAsync();

        var source = query
            .OrderByDescending(q => q.DateCreated)
            .ThenBy(q => q.DateUpdated);

        return await PaginatedList<T>.CreateAsync(source, 1, totalCount, totalCount);
    }

    public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _entities
            .Where(q => !q.IsDeleted)
            .Where(q => q.Id == id);

        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<T> InsertAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));


        await _entities.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<List<T>> InsertRangeAsync(List<T> entities)
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        return entities;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.DateUpdated = DateTime.UtcNow;

        _entities.Update(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<List<T>> UpdateRangeAsync(List<T> entities)
    {
        context.UpdateRange(entities);
        await context.SaveChangesAsync();

        return entities;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _entities.SingleOrDefaultAsync(s => s.Id == id);

        if (entity == null) throw new Exception("Entity not found");
        entity.IsDeleted = true;

        await UpdateAsync(entity);
    }

    public async Task DeleteRangeAsync(List<Guid> ids)
    {
        var entities = new List<T>();

        foreach (var id in ids)
        {
            var entity = await _entities.SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null) throw new Exception("Entity not found");
            entity.IsDeleted = true;
            entities.Add(entity);
        }

        await UpdateRangeAsync(entities);
    }
}