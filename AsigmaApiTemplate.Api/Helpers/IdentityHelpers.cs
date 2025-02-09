using AsigmaApiTemplate.Api.AppSettings.Options;
using IdentityModel.Client;

namespace AsigmaApiTemplate.Api.Helpers;

public static class IdentityHelpers
{
    public static async Task<string> GetAccessTokenAsync()
    {
        var client = new HttpClient();

        var identitySettings = GetEnvAuthVariables();

        var disco = await client.GetDiscoveryDocumentAsync(identitySettings.AuthAddress);

        if (disco.IsError) throw new Exception(disco.Error);

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = identitySettings.ClientId,
            Scope = identitySettings.Scope,
            GrantType = identitySettings.GrantType,
            ClientSecret = identitySettings.ClientSecret
        });

        if (tokenResponse.IsError) throw new Exception(tokenResponse.Error);

        return tokenResponse.AccessToken!;
    }

    private static IdentitySettings GetEnvAuthVariables()
    {
        var config = ConfigHelpers.Load();

        var configSection = config.GetSection(IdentitySettings.Identity);
        var identitySettings = configSection.Get<IdentitySettings>();

        return identitySettings!;
    }
}