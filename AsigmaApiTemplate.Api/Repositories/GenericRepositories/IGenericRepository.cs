using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;

namespace AsigmaApiTemplate.Api.Repositories.GenericRepositories;

public interface IGenericRepository<T> where T : IEntity
{
    Task<PaginatedList<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Guid id);
    
    Task<T> InsertAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task DeleteAsync(Guid id);
}