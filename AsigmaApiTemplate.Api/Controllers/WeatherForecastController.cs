using System.Net.Mime;
using AsigmaApiTemplate.Api.AppSettings.Options;
using AsigmaApiTemplate.Api.Dtos;
using AsigmaApiTemplate.Api.Helpers;
using AsigmaApiTemplate.Api.Models;
using AsigmaApiTemplate.Api.SearchObjects;
using AsigmaApiTemplate.Api.Services.GenericServices;
using AsigmaApiTemplate.Api.Services.Requests;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace AsigmaApiTemplate.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/weather/forecast")]
[Authorize]
public class WeatherForecastController(
    IGenericService<WeatherForecast> weatherForecastService,
    IRequestService requestService,
    IOptions<ServiceBaseUrlOptions> serviceBaseUrlOptions)
    : ControllerBase
{
    private readonly ServiceBaseUrlOptions _serviceBaseUrlOptions = serviceBaseUrlOptions.Value;

    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(SearchResponse<WeatherForecast>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetWeatherForecastsV1Async([FromQuery] SearchWeatherForecast search)
    {
        Log.Information("Received request for weather forecast version 1.0: {@SearchWeatherForecast}", search);
        

        
        try
        {
            var predicate = PredicateBuilder.BuildWeatherForecastPredicate(search);
            var result = await weatherForecastService.SearchAsync(search.Page, search.PageSize, predicate,
                source => source.Include(s => s.AffectedRegions));
            var totalPages = Math.Ceiling(result.totalCount / search.PageSize);

            var paginationMetadata = new PaginationMetadata
            {
                PageSize = search.PageSize,
                CurrentPage = search.Page,
                TotalItems = result.totalCount,
                TotalPages = totalPages,
                HasNextPage = search.Page < totalPages,
                HasPreviousPage = search.Page > 1
            };

            return Ok(new SearchResponse<WeatherForecast>
            {
                PaginationMetadata = paginationMetadata,
                Data = result.data
            });
        }
        catch (Exception e)
        {
            Log.Error(
                "Failed to get weather forecast for version 1.0: {ExceptionMessage} search: {@SearchWeatherForecast}",
                e.Message, search);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.1")]
    [ProducesResponseType(typeof(SearchResponse<WeatherForecast>), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetWeatherForecastsV2Async([FromQuery] SearchWeatherForecast search)
    {
        Log.Information("Received request for weather forecast version 1.1: {@SearchWeatherForecast}", search);

        try
        {
            var predicate = PredicateBuilder.BuildWeatherForecastPredicate(search);
            var result = await weatherForecastService.SearchAsync(search.Page, search.PageSize, predicate);
            var modifiedResult = result.data.Select(q =>
            {
                q.Summary = "I am version two's summary";
                return q;
            }).ToList();
            var totalPages = Math.Ceiling(result.totalCount / search.PageSize);

            var paginationMetadata = new PaginationMetadata
            {
                PageSize = search.PageSize,
                CurrentPage = search.Page,
                TotalItems = result.totalCount,
                TotalPages = totalPages,
                HasNextPage = search.Page < totalPages,
                HasPreviousPage = search.Page > 1
            };
            return Ok(new SearchResponse<WeatherForecast>
            {
                PaginationMetadata = paginationMetadata,
                Data = modifiedResult
            });
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
            var weatherForecast = await weatherForecastService.GetByIdAsync(id);
            if (weatherForecast == null)
            {
                Log.Error("Weather forecast with ID {Id} not found.", id);
                return NotFound($"Weather forecast with ID {id} not found.");
            }

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
            var result = await weatherForecastService.InsertAsync(model);
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
            var result = await weatherForecastService.UpdateAsync(model);
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
            await weatherForecastService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error("Failed to delete weather forecast with Id {id}: {Exception}", id, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [MapToApiVersion("1.0")]
    [HttpGet("api")]
    public async Task<IActionResult> SendApiRequestAsync()
    {
        Log.Information("Received request to get weather forecast");

        try
        {
            await requestService.GetAsync("weather/update", _serviceBaseUrlOptions.WeatherStation,
                new { day = DateTime.Today });

            return Ok();
        }
        catch (Exception e)
        {
            Log.Error("Api request failed: {Exception}", e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}