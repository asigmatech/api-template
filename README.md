# ASP.NET Core Web API Template

Elevate your API development workflow with a ready-to-use ASP.NET Core Web API template. Built on **.NET 8.0**, it focuses on **best practices** and **rapid setup**, so you can ship features faster.

## Why This Template?
- **Repository/Service Pattern** – Streamlined data access using a pattern based on separation of concerns.
- **Serilog-Integrated Logging** – Easily ship structured logs for better observability.
- **Auto DI Registration** – Mappers, Named HttpClients, and more get injected with zero hassle.
- **Swagger Integration** – Generate interactive API documentation on the fly.
- **Handy Helpers & Extensions** – Common functionality for everyday tasks, saving you time.

---

## What's New in Version 2.0.0
1. **Powerful data retrieval methods**
    - Generic and reusable data retrieval methods with `Include` and `ThenInclude` methods to pull related entities in one query
2. **External API Calls**
    - Pre-configured DI registration for Named `HttpClients`
    - Pre-configured Request service that uses the recommended`HttpClientFactory` to make API calls
3. **IOptions Pattern**
    - Simplify configuration management and testing with typed settings
4. **Hangfire Retries**
    - Built-in retry configuration for background tasks, handling transient failures gracefully
5. **Enhanced Log Enricher**
    - Enrich logs with the API name for better traceability
6. **IdentityHelper (OAuth2)**
    - Methods that retrieve access tokens using any discovery document, reducing duplicated code

---

## Setup Instructions
1. **Install the Template**
   ```bash
   dotnet new install ASIGMA.AspNetCore.API.Template::1.0.5
   ```
2. **Create a New Project**
   ```bash
   dotnet new asigmaaspwebapi -n MyAwesomeApi
   ```
3. **Restore & Run**
   ```bash
   cd MyAwesomeApi
   dotnet restore
   dotnet run
   ```
4. **Explore Swagger**
    - Navigate to `https://localhost:<port>/swagger` to view the auto-generated API docs.

---

## License
This template is distributed under the **MIT License**, allowing you to use, modify, and distribute it freely for both commercial and personal projects.

Enjoy building your next API with minimal setup and maximum productivity! Feel free to open **issues** or **pull requests** to help improve this template.
