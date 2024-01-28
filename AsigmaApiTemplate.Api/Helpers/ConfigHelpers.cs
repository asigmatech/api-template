namespace AsigmaApiTemplate.Api.Helpers;

public static class ConfigHelpers
{
    public static IConfigurationRoot Load(Env? env = null)
    {
        var configurationBuilder = new ConfigurationBuilder();
        AddJsonFiles(configurationBuilder, env);
        return configurationBuilder.Build();
    }

    private static void AddJsonFiles(IConfigurationBuilder configurationBuilder, Env? env = null)
    {
        env ??= EnvHelper.GetEnvironment();
        configurationBuilder
            .AddJsonFile($"AppSettings/appsettings.json")
            .AddJsonFile($"AppSettings/appsettings.{env}.json");
    }
    
    public enum Env
    {
        Development,
        Test,
        Staging,
        Production
    }

    public static class EnvHelper
    {
        public static Env GetEnvironment()
        {
            var environmentName =   Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
    
            ArgumentNullException.ThrowIfNull(environmentName);
            return (Env)Enum.Parse(typeof(Env), environmentName);
        }
    }
}