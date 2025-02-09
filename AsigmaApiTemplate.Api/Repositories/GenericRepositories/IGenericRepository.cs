using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace AsigmaApiTemplate.Api.Repositories.GenericRepositories;

public interface IGenericRepository<T> where T : IEntity
{
    Task<(List<T> data, double totalCount)> SearchAsync(int page, int pageSize, Expression<Func<T, bool>>? filters,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);

    Task<List<T>> SearchAsync(Expression<Func<T, bool>>[]? filters,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);

    Task<PaginatedList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
    Task<T> InsertAsync(T entity);
    Task<List<T>> InsertRangeAsync(List<T> entities);
    Task<T> UpdateAsync(T entity);
    Task<List<T>> UpdateRangeAsync(List<T> entities);
    Task DeleteAsync(Guid id);
    Task DeleteRangeAsync(List<Guid> ids);
}