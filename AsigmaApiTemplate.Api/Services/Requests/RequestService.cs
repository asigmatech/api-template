using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Services.Identity;
using IdentityModel.Client;

namespace AsigmaApiTemplate.Api.Services.Requests;

public class RequestService(IHttpClientFactory httpClientFactory, IIdentityHelperService identityHelperService)
    : IRequestService
{
    public async Task<string> GetAsync(string endpoint, string clientName, object? parameters = null)
    {
        var httpClient = httpClientFactory.CreateClient(clientName);
        var token = await identityHelperService.GetAccessTokenAsync();
        httpClient.SetBearerToken(token);

        var url = string.IsNullOrEmpty(parameters?.ToQueryString())
            ? endpoint
            : $"{endpoint}{parameters.ToQueryString()}";

        try
        {
            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException httpEx)
        {
            throw new Exception($"HTTP request failed: {httpEx.Message}", httpEx);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while making the API request.", ex);
        }
    }

    public async Task<string> PostAsync(string endpoint, string clientName, object data)
    {
        var httpClient = httpClientFactory.CreateClient(clientName);
        var token = await identityHelperService.GetAccessTokenAsync();
        httpClient.SetBearerToken(token);

        var response = await httpClient.PostAsJsonAsync(endpoint, data);

        if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> PutAsync(string endpoint, string clientName, Guid id, object data)
    {
        var httpClient = httpClientFactory.CreateClient(clientName);
        var token = await identityHelperService.GetAccessTokenAsync();
        httpClient.SetBearerToken(token);
        var url = $"{endpoint}/{id}";

        var response = await httpClient.PutAsJsonAsync(url, data);
        if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task DeleteAsync(string endpoint, string clientName, Guid id)
    {
        var httpClient = httpClientFactory.CreateClient(clientName);
        var token = await identityHelperService.GetAccessTokenAsync();
        httpClient.SetBearerToken(token);
        var requestUri = $"{endpoint}/{id}";
        var response = await httpClient.DeleteAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error deleting resource: {response.StatusCode} - {errorContent}");
        }
    }
}