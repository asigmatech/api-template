using Hangfire;
using HangfireBasicAuthenticationFilter;
using System.Reflection;


namespace AsigmaApiTemplate.Api.Extensions;

public static class HangfireExtensions
{
    public static void UseHangFireJobs(this IApplicationBuilder app)
    {
        var projectNameSpan = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
        var projectName = projectNameSpan[..^4];

        //You can change your path to what you want. The default is
        // "/<ProjectName>/jobs"

        app.UseHangfireDashboard($"/{projectName}/jobs", new DashboardOptions
        {
            DashboardTitle = $"{projectName} Jobs",
            DarkModeEnabled = true,
            DisplayStorageConnectionString = false,
            AppPath = "https://localhost:7249/swagger/index.html",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = "<Replace with own User>",
                    Pass = "<Replace with own Password>"
                }
            }
        });
    }
}