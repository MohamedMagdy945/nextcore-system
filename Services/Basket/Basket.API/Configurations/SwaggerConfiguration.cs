using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AppCoreSystem.API.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                   new OpenApiInfo
                   {
                       Version = "v1",
                       Title = "Basket API",
                       Description = "An ASP.NET Core Web API for managing basket v1 micro-services in commerce application",
                       Contact = new OpenApiContact
                       {
                           Name = "Mohamed Magdy",
                           Email = "mohamedmagdy000022@gmail.com",
                       }
                   });
            options.SwaggerDoc("v2",
                new OpenApiInfo
                {
                    Version = "v2",
                    Title = "Basket API",
                    Description = "An ASP.NET Core Web API for managing basket v2 micro-services in commerce application",
                    Contact = new OpenApiContact
                    {
                        Name = "Mohamed Magdy",
                        Email = "mohamedmagdy000022@gmail.com",
                    }
                });

            const string securityScheme = "Bearer";

            options.AddSecurityDefinition(securityScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = securityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });


        });

        return services;
    }

    public static WebApplication UseSwaggerDocumentation(
       this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "Basket API V2");
            options.RoutePrefix = "swagger";


            options.DisplayRequestDuration();
            options.EnablePersistAuthorization();
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1);
            options.EnableFilter();
            options.EnableDeepLinking();
        });

        return app;
    }
}