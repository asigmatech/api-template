using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;

namespace AsigmaApiTemplate.Api.Services.GenericServices;

public interface IGenericService<T> where T:IEntity
{
    Task<PaginatedList<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Guid id);
    
    Task<T> InsertAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task DeleteAsync(Guid id);
}