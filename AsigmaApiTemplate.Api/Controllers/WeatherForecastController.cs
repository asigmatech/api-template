using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.Mime;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.Services.GenericServices;

namespace AsigmaApiTemplate.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/weather/forecast")]
public class WeatherForecastController : ControllerBase
{
    private readonly IGenericService<WeatherForecast> _service;

    public WeatherForecastController(IGenericService<WeatherForecast> service)
    {
        _service = service;
    }

    [MapToApiVersion("1.0")]
    [HttpGet]
    public async Task<IActionResult> GetWeatherForecastsV1Async()
    {
        Log.Information("Received request for weather forecast version 1.0");

        try
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Failed to get weather forecast for version 1.0: {ExceptionMessage}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.1")]
    [HttpGet]
    public async Task<IActionResult> GetWeatherForecastsV2Async()
    {
        Log.Information("Received request for weather forecast version 1.1");

        try
        {
            var result = await _service.GetAllAsync();
            var modifiedResult = result.Select(wf => 
            {
                wf.Summary = "I am version two the elusive";
                return wf;
            }).ToList();
            return Ok(modifiedResult);
        }
        catch (Exception e)
        {
            Log.Error("Failed to get weather forecast for version 1.1: {ExceptionMessage}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.0")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        Log.Information("Received request to retrieve weather forecast with Id ({Id}):", id);

        try
        {
            var weatherForecast = await _service.GetByIdAsync(id);
            return Ok(weatherForecast);
        }
        catch (Exception e)
        {
            Log.Error("Failed to get weather forecast with Id: {Id}, {ExceptionMessage}", id, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.0")]
    [Consumes(MediaTypeNames.Application.Json)]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] WeatherForecast model)
    {
        Log.Information("Received request to create a weather forecast: {@WeatherForecast}", model);

        try
        {
            var result = await _service.InsertAsync(model);
            Log.Information("Weather forecast created successfully Id: {Id}", model.Id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Failed to create weather forecast: {ExceptionMessage}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.0")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] WeatherForecast model)
    {
        Log.Information("Received request to update weather forecast with Id ({Id}):", model.Id);

        if (id != model.Id)
        {
            Log.Error("ID in the route does not match the ID in the body.");
            return BadRequest("ID in the route does not match the ID in the body.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest("Model Invalid");
        }

        try
        {
            var result = await _service.UpdateAsync(model);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Error("Failed to update weather forecast with Id {id}: {Exception}", id, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.0")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        Log.Information("Received request to delete weather forecast with Id: {Id}", id);

        if (id == Guid.Empty)
        {
            return BadRequest(id);
        }

        try
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error("Failed to delete weather forecast with Id {id}: {Exception}", id, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
