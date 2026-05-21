using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace Common.Logging
{
    public static class CustomRequestLogging
    {
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
}
