using AsigmaApiTemplate.Api.Dtos;
using AsigmaApiTemplate.Api.Models;
using AutoMapper;

namespace AsigmaApiTemplate.Api.MapperProfiles;

public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<WeatherForecast, WeatherForecastDto>();
    }
}

