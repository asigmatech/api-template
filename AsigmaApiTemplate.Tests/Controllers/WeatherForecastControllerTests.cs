using AsigmaApiTemplate.Api;
using AsigmaApiTemplate.Api.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AsigmaApiTemplate.Tests.Controllers;

public class WeatherForecastControllerTests
{
    private readonly WeatherForecastController _weatherForecastController;

    public WeatherForecastControllerTests()
    {
        var logger = A.Fake<ILogger<WeatherForecastController>>();
        var httpContextAccessor = A.Fake<IHttpContextAccessor>();
        _weatherForecastController = new WeatherForecastController(logger, httpContextAccessor);
    }

    [Fact]
    public void WeatherForecastController_GetWeatherV1_ReturnsFiveWeatherObjects()
    {
        //Arrange
        
        
        //Act
        var result = _weatherForecastController.GetWeatherV1();
        var weatherForecasts = result as WeatherForecast[] ?? result.ToArray();

        //Assert
        weatherForecasts.Should().HaveCount(5);
        weatherForecasts.Should()
            .OnlyHaveUniqueItems();
        weatherForecasts.Should().NotContainNulls();

        weatherForecasts.ToList().ForEach(forecast =>
        {
            forecast.Date.Should().BeOnOrAfter(DateOnly.FromDateTime(DateTime.Now)
            );
            forecast.TemperatureC.Should().BeInRange(-20, 55);
            forecast.Summary.Should().NotBeNull();
        });
    }
}