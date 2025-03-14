namespace AsigmaApiTemplate.Api.Models;

public class WeatherForecast : BaseModel
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }

    public List<Region> AffectedRegions { get; set; } = [];
}