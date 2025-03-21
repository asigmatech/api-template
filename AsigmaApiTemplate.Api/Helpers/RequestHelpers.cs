using System.Web;
using AsigmaApiTemplate.Api.AppSettings.Options;

namespace AsigmaApiTemplate.Api.Helpers;

public static class RequestHelpers
{
    public static string ToQueryString(this object? obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }

        var properties = from p in obj.GetType().GetProperties()
            let value = p.GetValue(obj, null)
            where value != null
            select p.Name + "=" + HttpUtility.UrlEncode(value?.ToString() ?? string.Empty);

        return "?" + string.Join("&", properties.ToArray());
    }


    public static string GetBaseUrl(this string baseUrl)
    {
        var config = ConfigHelpers.Load();
        var result = config
            .GetSection(ServiceBaseUrlOptions.ServiceBaseUrls)
            .GetSection(baseUrl).Value;

        return result!;
    }
}