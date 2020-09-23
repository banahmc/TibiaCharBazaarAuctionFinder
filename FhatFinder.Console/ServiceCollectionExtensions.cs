using FhatFinder.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers;

namespace FhatFinder.Console
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsole(this IServiceCollection services)
        {
            services.AddSerilog();
            services.AddTransient<IAuctionMatchingService, AuctionMatchingService>();
            services.AddTransient<IFHatFinderApp, FHatFinderApp>();
            return services;
        }

        private static IServiceCollection AddSerilog(this IServiceCollection services)
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.RollingFile(
                    "..\\..\\..\\..\\logs\\FHatFinder.log",
                    Serilog.Events.LogEventLevel.Debug,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} <{ThreadId}><{ThreadName}> [{Level:u3}] {Message:lj} {NewLine}{Exception}")
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "ServiceWorkerThread")
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddSerilog(serilogLogger, dispose: true);
            });

            return services;
        }
    }
}
