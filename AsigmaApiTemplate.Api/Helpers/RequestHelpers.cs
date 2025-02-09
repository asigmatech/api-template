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
            where p.GetValue(obj, null) != null
            select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

        return "?" + string.Join("&", properties.ToArray());
    }

    public static string GetBaseUrl(this string baseUrl)
    {
        var config = ConfigHelpers.Load();
        var result = config
            .GetSection(BaseUrls.ServiceBaseUrlsSection)
            .GetSection(baseUrl).Value;

        return result!;
    }

    public static Dictionary<string, string?> GetAllBaseUrls()
    {
        var config = ConfigHelpers.Load();
        var baseUrlsSection = config.GetSection(BaseUrls.ServiceBaseUrlsSection);

        // 1. Check if the section exists
        if (!baseUrlsSection.Exists())
        {
            return new Dictionary<string, string?>();
        }

        // 2. Retrieve the children; might be empty
        var children = baseUrlsSection.GetChildren().ToList();

        // 3. If there are no children, also return an empty dictionary
        if (children.Count == 0)
        {
            return new Dictionary<string, string?>();
        }

        // If we’re here, we have children; build the dictionary
        var result = children.ToDictionary(child => child.Key, child => child.Value);
        return result;
    }


}