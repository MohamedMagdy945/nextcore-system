using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Common.Logging;

public static class LoggingConfiguration
{

    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .CreateBootstrapLogger();
    }


    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }
    public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app)
    {

        return app.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (ctx, _, ex) =>
               ex != null ? LogEventLevel.Error :
               ctx.Response.StatusCode >= 500 ? LogEventLevel.Error :
               ctx.Response.StatusCode >= 400 ? LogEventLevel.Warning :
               LogEventLevel.Information;

            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} -> {StatusCode} in {Elapsed:0.0000} ms";
        });
    }
}