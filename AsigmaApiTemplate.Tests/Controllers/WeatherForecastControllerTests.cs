﻿using System.Linq.Expressions;
using AsigmaApiTemplate.Api;
using AsigmaApiTemplate.Api.Controllers;
using AsigmaApiTemplate.Api.Dtos;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.SearchObjects;
using AsigmaApiTemplate.Api.Services.GenericServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AsigmaApiTemplate.Tests.Controllers;

public class WeatherForecastControllerTests
{
    private readonly WeatherForecastController _weatherForecastController;
    private readonly IGenericService<WeatherForecast> _weatherForecastService;

    public WeatherForecastControllerTests()
    {
        _weatherForecastService = A.Fake<IGenericService<WeatherForecast>>();
        _weatherForecastController = new WeatherForecastController(_weatherForecastService);
    }

    [Fact]
    public async Task WeatherForecastController_GetWeatherForecastsV1Async_ReturnsOk_WithWeatherForecasts()
    {
        // Arrange
        var search = new SearchWeatherForecast { Page = 1, PageSize = 10 };
        ICollection<WeatherForecast> weatherForecasts = new List<WeatherForecast>
        {
            new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 25,
                Summary = "Sunny"
            }
        };

        var predicate = PredicateBuilder.BuildWeatherForecastPredicate(search);

        A.CallTo(() => _weatherForecastService.SearchAsync(search.Page, search.PageSize, predicate))
            .Returns(Task.FromResult((data: weatherForecasts, totalCount: weatherForecasts.Count * 1.0)));

        // Act
        var result = await _weatherForecastController.GetWeatherForecastsV1Async(search);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async Task WeatherForecastController_GetByIdAsync_ReturnsOk_WhenWeatherForecastFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var weatherForecast = A.Fake<WeatherForecast>();
        A.CallTo(() => _weatherForecastService.GetByIdAsync(id)).Returns(Task.FromResult<WeatherForecast?>(weatherForecast));

        // Act
        var result = await _weatherForecastController.GetByIdAsync(id);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        objectResult.Value.Should().BeEquivalentTo(weatherForecast);
    }

    [Fact]
    public async Task WeatherForecastController_GetByIdAsync_ReturnsNotFound_WhenWeatherForecastDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _weatherForecastService.GetByIdAsync(id)).Returns(Task.FromResult<WeatherForecast?>(null));

        // Act
        var result = await _weatherForecastController.GetByIdAsync(id);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task WeatherForecastController_CreateAsync_ReturnsOk_WhenWeatherForecastSuccessfullyCreated()
    {
        // Arrange
        var weatherForecast = A.Fake<WeatherForecast>();
        A.CallTo(() => _weatherForecastService.InsertAsync(weatherForecast)).Returns(Task.FromResult(weatherForecast));

        // Act
        var result = await _weatherForecastController.CreateAsync(weatherForecast);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        objectResult.Value.Should().BeEquivalentTo(weatherForecast);
    }

    [Fact]
    public async Task WeatherForecastController_UpdateAsync_ReturnsOk_WhenWeatherForecastSuccessfullyUpdated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var weatherForecast = A.Fake<WeatherForecast>();
        weatherForecast.Id = id;
        A.CallTo(() => _weatherForecastService.UpdateAsync(weatherForecast)).Returns(Task.FromResult(weatherForecast));

        // Act
        var result = await _weatherForecastController.UpdateAsync(id, weatherForecast);

        // Assert
        var objectResult = result.Should().BeOfType<OkObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        objectResult.Value.Should().BeEquivalentTo(weatherForecast);
    }

    [Fact]
    public async Task WeatherForecastController_UpdateAsync_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var routeId = Guid.NewGuid();
        var bodyId = Guid.NewGuid(); // Different ID in the body
        var weatherForecast = A.Fake<WeatherForecast>();
        weatherForecast.Id = bodyId;

        // Act
        var result = await _weatherForecastController.UpdateAsync(routeId, weatherForecast);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task WeatherForecastController_DeleteAsync_ReturnsOk_WhenWeatherForecastSuccessfullyDeleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _weatherForecastService.DeleteAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _weatherForecastController.DeleteAsync(id);

        // Assert
        var objectResult = result.Should().BeOfType<OkResult>().Subject;
    }

    [Fact]
    public async Task WeatherForecastController_DeleteAsync_ReturnsNotFound_WhenWeatherForecastDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _weatherForecastService.DeleteAsync(id)).Throws(new Exception("Internal server error"));

        // Act
        var result = await _weatherForecastController.DeleteAsync(id);

        // Assert
        result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}
