using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Common.Logging;

public static class LoggingConfiguration
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] " +
            "{Message:lj}{NewLine}{Exception}")
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .CreateBootstrapLogger();
    }

    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        var loggingOptions = new LoggingOptions();

        builder.Configuration
           .GetSection("LoggingOptions")
           .Bind(loggingOptions);

        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ServiceName", loggingOptions.ServiceName)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .ReadFrom.Configuration(builder.Configuration);

        if (loggingOptions.UseConsole)
        {
            loggerConfiguration.WriteTo.Console(
               outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] " +
               "{Message:lj} [{CorrelationId}]{NewLine}{Exception}"
            );
        }

        if (loggingOptions.UseSeq)
        {
            loggerConfiguration.WriteTo.Seq(loggingOptions.SeqUrl);
        }

        if (loggingOptions.UseElasticsearch)
        {
            loggerConfiguration.WriteTo.Elasticsearch(
             new[] { new Uri(loggingOptions.ElasticsearchUrl) },
             opts =>
             {
                 opts.DataStream = new DataStreamName(
                     loggingOptions.ServiceName,
                     "logs",
                     "application");
             });
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}