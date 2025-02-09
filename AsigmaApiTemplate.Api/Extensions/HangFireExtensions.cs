using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace AsigmaApiTemplate.Api.Extensions;

public static class HangfireExtensions
{
    public static void UseHangFireJobs(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("<Replace with own route>", new DashboardOptions
        {
            DashboardTitle = "<Replace with own Title>",
            DarkModeEnabled = true,
            DisplayStorageConnectionString = false,
            AppPath = "https://localhost:7249/swagger/index.html", //Change this if need be
            Authorization =
            [
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = "<Replace with own User>",
                    Pass = "<Replace with own Password>"
                }
            ]
        });
    }
}