using System.Reflection;
using AsigmaApiTemplate.Api.Controllers;
using FluentAssertions.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsigmaApiTemplate.Tests.Generic;

public class AuthTests
{
    [Theory, MemberData(nameof(GetAllControllerTypes))]
    public void Controllers_HaveAuthorizeAttribute(Type controllerType)
    {
        // Arrange
        var authorizeAttribute = controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), true).FirstOrDefault();

        // Assert
        Assert.NotNull(authorizeAttribute);
    }

    public static IEnumerable<object[]> GetAllControllerTypes()
    {
        var assembly = Assembly.GetAssembly(typeof(WeatherForecastController));
        var controllers = AllTypes.From(assembly).ThatDeriveFrom<ControllerBase>()
            .Select(type => new object[] { type });

        return controllers;
    }
}
