using AsigmaApiTemplate.Api.Data;
using AsigmaApiTemplate.Api.Extensions;
using AsigmaApiTemplate.Api.Helpers;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var errorString = string.Join(", ", errors);
            Log.Error("Model validation failed: {Errors}", errorString);
            var result = new BadRequestObjectResult(context.ModelState);
 
            return result;
        };
    });

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
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDependencyInjection();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //TODO
        //Enter the details of the identity authority
        
        options.Authority = "https://#";
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var configuration = ConfigHelpers.Load();
var connectionString = configuration.GetConnectionString("DefaultConnection");


builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!);

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString!));

Log.Logger = new LoggerConfiguration()
    .Enrich.With(new LogEnricher())
    .ReadFrom.Configuration(configuration)
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

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("*");

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();