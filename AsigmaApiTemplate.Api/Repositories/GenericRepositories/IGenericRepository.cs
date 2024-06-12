using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;

namespace AsigmaApiTemplate.Api.Repositories.GenericRepositories;

public interface IGenericRepository<T> where T : IEntity
{
    Task<PaginatedList<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);

    Task<T> InsertAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task DeleteAsync(Guid id);
    Task<(ICollection<T> data, double totalCount)> SearchAsync(int page, int pageSize, params Expression<Func<T, bool>>[] filters);

}