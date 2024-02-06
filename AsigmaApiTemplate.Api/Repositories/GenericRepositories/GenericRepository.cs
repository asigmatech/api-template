using AsigmaApiTemplate.Api.Data;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AsigmaApiTemplate.Api.Repositories.GenericRepositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _entities;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _entities.SingleOrDefaultAsync(s => s.Id == id);

        if (entity == null) throw new Exception("Entity not found");
        entity.IsDeleted = true;

        await UpdateAsync(entity);

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

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _entities
            .Where(q => !q.IsDeleted)
            .FirstOrDefaultAsync(s => s.Id == id);

        return entity;
    }

    public async Task<T> InsertAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;

    }

    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.DateUpdated = DateTime.UtcNow;

        _entities.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
}