using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AsigmaApiTemplate.Api.Helpers
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var projectNameSpan = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            var projectName = projectNameSpan[..^4];

            var info = new OpenApiInfo
            {
                Title = projectName,
                Version = description.ApiVersion.ToString(),
                Description = $"An api for {projectName[..^4]}",
                Contact = new OpenApiContact { Name = "ASIGMA Technology Services", Email = "asigmatech@gmail.com" },
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}