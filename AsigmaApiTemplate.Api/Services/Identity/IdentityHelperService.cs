using AsigmaApiTemplate.Api.AppSettings.Options;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace AsigmaApiTemplate.Api.Services.Identity;

public class IdentityHelperService(IOptions<IdentityOptions> identityOptions, HttpClient httpClient)
    : IIdentityHelperService
{
    private readonly IdentityOptions _identitySettings = identityOptions.Value;

    public async Task<string> GetAccessTokenAsync()
    {
        var disco = await httpClient.GetDiscoveryDocumentAsync(_identitySettings.AuthAddress);

        if (disco.IsError)
            throw new Exception(disco.Error);

        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = _identitySettings.ClientId,
            Scope = _identitySettings.Scope,
            GrantType = _identitySettings.GrantType,
            ClientSecret = _identitySettings.ClientSecret
        });

        if (tokenResponse.IsError)
            throw new Exception(tokenResponse.Error);

        return tokenResponse.AccessToken!;
    }
}