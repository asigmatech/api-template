using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore.Query;

namespace AsigmaApiTemplate.Api.Services.GenericServices;

public class GenericService<T>(IGenericRepository<T> repository) : IGenericService<T>
    where T : IEntity
{
    public async Task<(List<T> data, double totalCount)> SearchAsync(int page, int pageSize,
        Expression<Func<T, bool>>? filters, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        return await repository.SearchAsync(page, pageSize, filters, includes);
    }

    public async Task<List<T>> SearchAsync(Expression<Func<T, bool>>[]? filters,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        return await repository.SearchAsync(filters, includes);
    }

    public async Task<PaginatedList<T>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        return await repository.GetByIdAsync(id, includeProperties);
    }


    public async Task<T> InsertAsync(T data)
    {
        return await repository.InsertAsync(data);
    }

    public async Task<List<T>> InsertRangeAsync(List<T> entities)
    {
        return await repository.InsertRangeAsync(entities);
    }

    public async Task<T> UpdateAsync(T data)
    {
        return await repository.UpdateAsync(data);
    }

    public async Task<List<T>> UpdateRangeAsync(List<T> entities)
    {
        return await repository.UpdateRangeAsync(entities);
    }

    public async Task DeleteAsync(Guid id)
    {
        await repository.DeleteAsync(id);
    }

    public async Task DeleteRangeAsync(List<Guid> ids)
    {
        await repository.DeleteRangeAsync(ids);
    }
}