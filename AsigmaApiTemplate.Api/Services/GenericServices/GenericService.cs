using System.Linq.Expressions;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.Repositories.GenericRepositories;

namespace AsigmaApiTemplate.Api.Services.GenericServices;

public class GenericService<T> : IGenericService<T> where T : IEntity
{
    private readonly IGenericRepository<T> _repository;

    public GenericService(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        return await _repository.GetByIdAsync(id, includeProperties);
    }


    public async Task<T> InsertAsync(T data)
    {
        return await _repository.InsertAsync(data);
    }
    public async Task<T> UpdateAsync(T data)
    {
        return await _repository.UpdateAsync(data);
    }
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
    public async Task<(ICollection<T> data, double totalCount)> SearchAsync(int page, int pageSize,
        params Expression<Func<T, bool>>[] filters)
    {
        return await _repository.SearchAsync(page, pageSize, filters);
    }
}