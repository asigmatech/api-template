namespace AsigmaApiTemplate.Api.AppSettings.Options;

public class IdentityOptions
{
    public const string Identity = "Identity";
    public string AuthAddress { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string Scope { get; set; } = string.Empty;

    public string GrantType { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;
}