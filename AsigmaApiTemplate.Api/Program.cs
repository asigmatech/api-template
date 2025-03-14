using AsigmaApiTemplate.Api.AppSettings.Options;
using AsigmaApiTemplate.Api.Data;
using AsigmaApiTemplate.Api.Extensions;
using AsigmaApiTemplate.Api.Helpers;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddApiVersioning().AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var envConfiguration = ConfigHelpers.Load();

//Add the Option Classes to the Dependency Injection Container. See Sample Controller for how to access your options.
//Read more here -> https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0

builder.Services.Configure<IdentityOptions>(envConfiguration.GetSection(IdentityOptions.Identity));
builder.Services.Configure<ServiceBaseUrlOptions>(envConfiguration.GetSection(ServiceBaseUrlOptions.ServiceBaseUrls));

var serviceBaseUrlSection =
    envConfiguration.GetSection(ServiceBaseUrlOptions.ServiceBaseUrls);

var serviceBaseUrlChildren = serviceBaseUrlSection.GetChildren().ToList();


//This registers all the URLs in the ServiceBaseUrls section of your appSettings to the DI container as named clients
//Read more here -> https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/ and
//Here -> https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
if (serviceBaseUrlChildren.Count > 0)
{
    foreach (var kvp in serviceBaseUrlChildren.ToDictionary(
                 child => child.Key,
                 child => child.Value))
    {
        builder.Services.AddHttpClient(kvp.Key,
                client =>
                {
                    if (kvp.Value != null) client.BaseAddress = new Uri(kvp.Value, UriKind.Absolute);
                })
            //This uses the default values for resilience attributes. To configure your own options, watch this: https://youtu.be/pgeHRp2Otlc?si=LbpmjtLv-d7knWrp
            .AddStandardResilienceHandler();
    }
}


builder.Services.AddAutoMapper(typeof(Program)); //Automatically registers all mappers to the DI container
builder.Services.AddDependencyInjection();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //Configure your Identity Authority in appsettings.json or appsettings.{env}.json

        var identityOptions = envConfiguration.GetSection(IdentityOptions.Identity).Get<IdentityOptions>();
        options.Authority = identityOptions!.AuthAddress;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "<Replace with the desired scope>");
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("*",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("content-type")
                .WithExposedHeaders("X.Pagination");
        });
});

var connectionString = envConfiguration.GetConnectionString("DefaultConnection");


builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString!));

builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseDefaultTypeSerializer()
        .UseSimpleAssemblyNameTypeSerializer()
        .UsePostgreSqlStorage(
            options => options.UseNpgsqlConnection(connectionString),
            new PostgreSqlStorageOptions()
        );
    GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
    {
        Attempts = 7, // Number of retry attempts
        DelaysInSeconds = [10, 20, 30, 1800, 3600, 7200, 10800], // Delays between retries in seconds
        OnAttemptsExceeded = AttemptsExceededAction.Fail, // Fails after retry limit exceeded
    });
});

builder.Services.AddHangfireServer();

Log.Logger = new LoggerConfiguration()
    .Enrich.With(new LogEnricher())
    .ReadFrom.Configuration(envConfiguration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

await app.UpdateDatabaseAsync();

if (!app.Environment.IsProduction())
{
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHangFireJobs();

app.UseCors("*");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapHealthChecks("/_health");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();