using System.Globalization;
using System.Text.RegularExpressions;

namespace AsigmaApiTemplate.Api.Extensions;

public static class Utils
{
    private const string PhoneNumberRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

    public static bool IsTelephone(this string number)
    {
        return !string.IsNullOrWhiteSpace(number) && number.Length >= 9;
    }

    public static Guid? GetRouteIdParameter(this IHttpContextAccessor accessor, string idParameter)
    {
        var idParameterResult = accessor.HttpContext?.Request.RouteValues[$"{idParameter}"]?.ToString();
        
        if (!string.IsNullOrWhiteSpace(idParameterResult))
        {
            return Guid.Parse(idParameterResult);
        }
        return null;
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                var domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }


    public static string TimeAgo(this DateTime dateTime)
    {
        string result;
        var timeSpan = DateTime.Now.Subtract(dateTime);

        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            result = $"{timeSpan.Seconds} seconds ago";
        }
        else if (timeSpan <= TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1
                ? $"about {timeSpan.Minutes} minutes ago"
                : "about a minute ago";
        }
        else if (timeSpan <= TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1
                ? $"about {timeSpan.Hours} hours ago"
                : "about an hour ago";
        }
        else if (timeSpan <= TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1
                ? $"about {timeSpan.Days} days ago"
                : "yesterday";
        }
        else if (timeSpan <= TimeSpan.FromDays(365))
        {
            result = timeSpan.Days > 30
                ? $"about {timeSpan.Days / 30} months ago"
                : "about a month ago";
        }
        else
        {
            result = timeSpan.Days > 365
                ? $"about {timeSpan.Days / 365} years ago"
                : "about a year ago";
        }

        return result;
    }
}