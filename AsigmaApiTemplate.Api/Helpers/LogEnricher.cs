using AsigmaApiTemplate.Api.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace AsigmaApiTemplate.Api.Helpers;

public class LogEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContextAccessor = new HttpContextAccessor();

        var correlationId = httpContextAccessor.GetRouteIdParameter("CorrelationId");
        var userId = httpContextAccessor.GetRouteIdParameter("UserId");
        var clientId = httpContextAccessor.GetRouteIdParameter("ClientId");
        var service= ConfigHelpers.Load().GetSection("Service").Value;
        

        var environment = ConfigHelpers.EnvHelper.GetEnvironment();

        var logEventProperties = new List<LogEventProperty>
        {
            CreateProperty("CorrelationId", correlationId ?? Guid.Empty, propertyFactory),
            CreateProperty("UserId", userId ?? Guid.Empty, propertyFactory),
            CreateProperty("ClientId", clientId ?? Guid.Empty, propertyFactory),
            CreateProperty("Environment", environment, propertyFactory),
            CreateProperty("Service", service!, propertyFactory)
        };

        foreach (var prop in logEventProperties)
        {
            logEvent.AddOrUpdateProperty(prop);
        }
    }

    private static LogEventProperty CreateProperty(string propertyName, object value,
        ILogEventPropertyFactory propertyFactory)
    {
        return propertyFactory.CreateProperty(propertyName, value);
    }
}