using AsigmaApiTemplate.Api.Models;

namespace AsigmaApiTemplate.Tests.Models;

public class WeatherForecastModelTests
{
     [Fact]
    public void WeatherForecastModel_AllRequiredFieldsArePresent_WhenInstantiated()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Now);
        var temperatureC = 20;
        var summary = "Sunny";

        var weatherForecast = new WeatherForecast
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary,
        };

        // Act
        var isValid = weatherForecast.Date != default &&
                      weatherForecast.TemperatureC != default;

        // Assert
        Assert.True(isValid);
    }

}