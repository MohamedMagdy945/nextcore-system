using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfigureation) =>
            {
                var environment = context.HostingEnvironment;
                loggerConfigureation.MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", environment.ApplicationName)
                    .Enrich.WithProperty("EnvironmentName", environment.EnvironmentName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.Console();

                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfigureation.MinimumLevel.Override("Catalog", Serilog.Events.LogEventLevel.Debug)
                        .MinimumLevel.Override("Basket", Serilog.Events.LogEventLevel.Debug)
                        .MinimumLevel.Override("Discount", Serilog.Events.LogEventLevel.Debug)
                        .MinimumLevel.Override("Ordering", Serilog.Events.LogEventLevel.Debug);
                }


            };
    }
}
