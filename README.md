# ASP.NET Core Web API Template

Streamline the setup of your ASP.NET Core Web API projects with this quick start template. Designed to accelerate your development process, it takes care of common configurations, allowing you to get your API up and running swiftly. This version of the API is built using the .NET 7.0 SDK.

## Considerations
You are using the Repository/Service design pattern.

## Features
1. **Generic Repositories and Services**
2. **Automatic Dependency Injection**
    - As long as a service inherits the generic service, dependency injection is automatic.
3. **Logging**
    - Logging using Serilog.
4. **Unit Testing**
    - Unit testing using XUnit, FluentAssertions, and FakeItEasy.
5. **API Versioning**
6. **Mapping**
    - Mapping using AutoMapper.
7. **Health Checks Configuration**
8. **Swagger Integration**
9. **Common Helper and Extension Methods**
    - Extension methods for Strings, Dates, and other foreseeable use cases.

## Improvements in version 1.0.4
1. **Generic Repositories and Services**
    - Introduced a generic search method.
    - Added an overload method for creating paginated lists from `IEnumerable` collections.
    - Modified the `GetByIdAsync` method to enable the ability to include related entities.
2. **Unit Testing**
    - Added tests for the `GenericRepository` and `GenericService`.
    - Added tests to ensure controllers are secured.
    - Included tests for the weather profile mapper.
3. **WeatherForecastController**
    - Added other CRUD methods.
4. **Pagination**
    - Added an overload method for creating paginated lists from IEnumerable collections.

## Setup Instructions

1. **Install Template**
    ```bash
    dotnet new install ASIGMA.AspNetCore.API.Template::1.0.4
    ```

2. **Update Connection String**
    Ensure you have a proper development environment connection string set in your `appsettings.Development.json` file. 

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=your_db;Username=your_user;Password=your_password;SSL Mode=Prefer;Trust Server Certificate=true"
      }
    }
    ```
## License
This project is licensed under the MIT License.
