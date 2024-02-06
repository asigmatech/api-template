using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AsigmaApiTemplate.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/[controller]")]


public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;

    }
    
    
    [MapToApiVersion("1.0")]
    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherV1()
    {
        //var correlationId= _httpContextAccessor.GetCorrelationId();

        //var CorrelationId = Guid.NewGuid();
        var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();


        return result;

    }
    
    [MapToApiVersion("1.1")]
    [HttpGet]
    public IEnumerable<WeatherForecast> GetWeatherV2()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "I am version two the elusive"
            })
            .ToArray();
    }
   
}