# ASP.NET Core Web API Template

Elevate your API development workflow with a ready-to-use ASP.NET Core Web API template. It follows industry best practices and provides all the essential boilerplate functionality, allowing you to focus on shipping features to your clients faster.

## Why This Template?
- **Repository/Service Pattern** – Streamlined data access using a pattern based on separation of concerns.
- **Serilog-Integrated Logging** – Easily ship structured logs for better observability.
- **Auto DI Registration** – Mappers, Named HttpClients, and more get injected with zero hassle.
- **Swagger Integration** – Generate interactive API documentation on the fly.
- **Handy Helpers & Extensions** – Common functionality for everyday tasks, saving you time.

---

## What's New in Version 2.0.1
1. **Powerful data retrieval methods**
   - Generic and reusable data retrieval methods with `Include` and `ThenInclude` methods to pull related entities in one query
2. **External API Calls**
   - Pre-configured DI registration for Named `HttpClients`
   - Pre-configured Request service that uses the recommended `HttpClientFactory` to make API calls
3. **IOptions Pattern**
   - Simplifies configuration management and testing with typed settings
4. **Hangfire Retries**
   - Built-in retry configuration for background tasks, handling transient failures gracefully
5. **Enhanced Log Enricher**
   - Enrich logs with the API name for better traceability
6. **IdentityHelper (OAuth2)**
   - Methods that retrieve access tokens using any discovery document, reducing duplicated code
7. **Extra Package Information**
   - Enhanced package metadata by specifying supported target frameworks, improving discoverability across the .NET ecosystem
   - Included [source repository](https://github.com/asigmatech/api-template) details to facilitate open-source feedback and community contributions

---

## Setup Instructions
1. **If you are upgrading from an older version, first uninstall the previous version to avoid conflicts**
   ```bash
   dotnet new uninstall ASIGMA.AspNetCore.API.Template
   ```
2. **Install the Template**
   ```bash
   dotnet new install ASIGMA.AspNetCore.API.Template
   ```
3. **Create a New Project**
   ```bash
   dotnet new asigmaaspwebapi -n MyAwesomeApi
   ```
4. **Restore & Run**
   ```bash
   cd MyAwesomeApi
   dotnet restore
   dotnet run
   ```
5. **Explore Swagger**
    - Navigate to `https://localhost:<port>/swagger` to view the auto-generated API docs.

---

## License
This template is distributed under the **MIT License**, allowing you to use, modify, and distribute it freely for both commercial and personal projects.

Enjoy building your next API with minimal setup and maximum productivity! Feel free to open **issues** or **pull requests** to help improve this template.

## Feedback & Contributing
This ASP.NET Core Web API Template is released as open source under the MIT license. Bug reports and contributions are welcome at the [GitHub repository](https://github.com/asigmatech/api-template).
