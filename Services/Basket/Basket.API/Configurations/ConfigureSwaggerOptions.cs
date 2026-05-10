using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Configurations;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MyApi V1",
            Version = "v1",
            Description = "A clean, versioned ASP.NET Core 9 Web API.",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "you@example.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
        options.SwaggerDoc("v2", new OpenApiInfo
        {
            Title = "MyApi V2",
            Version = "v2",
            Description = "A clean, versioned ASP.NET Core 9 Web API.",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "you@example.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
    }
}