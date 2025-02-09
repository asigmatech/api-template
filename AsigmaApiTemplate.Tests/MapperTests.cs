using AsigmaApiTemplate.Api.Dtos;
using AsigmaApiTemplate.Api.Models;
using AutoMapper;

namespace AsigmaApiTemplate.Tests;

public class MappersTests
{
    [Fact]
    public void WeatherForecastProfileMappersTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WeatherForecast, WeatherForecastDto>(MemberList.Source)
                .ForSourceMember(src => src.DateCreated, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.DateUpdated, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.AffectedRegions, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.IsDeleted, opt => opt.DoNotValidate());
        });
        config.AssertConfigurationIsValid();
    }
}