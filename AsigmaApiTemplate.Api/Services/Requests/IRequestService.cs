namespace AsigmaApiTemplate.Api.Services.Requests;

public interface IRequestService
{
    Task<string> GetAsync(string endpoint, string clientName, object? parameters = null);  
    Task<string> PostAsync(string endpoint, string clientName, object data);
    Task<string> PutAsync(string endpoint, string clientName,Guid id, object data);
    Task DeleteAsync(string endpoint, string clientName, Guid id);
}