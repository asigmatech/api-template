using AsigmaApiTemplate.Api.Dtos;

namespace AsigmaApiTemplate.Api.SearchObjects;

public class SearchWeatherForecast:Pagination
{
    public Guid? Id { get; set; }
    public DateOnly? Date { get; set; }
    public int? TemperatureC { get; set; }
}