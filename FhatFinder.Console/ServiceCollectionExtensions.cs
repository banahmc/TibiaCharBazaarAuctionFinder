using FhatFinder.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

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
                .WriteTo.RollingFile("FHatFinder.log")
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddSerilog(serilogLogger, dispose: true);
            });

            return services;
        }
    }
}
