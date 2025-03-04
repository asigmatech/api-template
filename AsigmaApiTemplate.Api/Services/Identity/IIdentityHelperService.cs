namespace AsigmaApiTemplate.Api.Services.Identity;

public interface IIdentityHelperService
{
    Task<string> GetAccessTokenAsync();

}