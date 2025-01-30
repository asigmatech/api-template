namespace AsigmaApiTemplate.Api.AppSettings.Options;

public class IdentitySettings
{
    public const string Identity = "Identity";
    public string AuthAddress { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string Scope { get; set; } = default!;

    public string GrantType { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;
}