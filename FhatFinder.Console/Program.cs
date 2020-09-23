using FhatFinder.Scraper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Console
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IServiceScope _serviceScope;
        private static ILogger<FHatFinderApp> logger;

        public static void Main(string[] args)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                System.Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                RegisterServices();

                _serviceScope = _serviceProvider.CreateScope();
                logger = _serviceScope.ServiceProvider.GetRequiredService<ILogger<FHatFinderApp>>();

                MainAsync(cts.Token).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger?.LogCritical($"An unhandled error has occurred: {ex.Message}\nError Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                DisposeServices();
            }
        }

        private static async Task MainAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting application...");
            var app = _serviceScope.ServiceProvider.GetRequiredService<IFHatFinderApp>();
            await app.Run(cancellationToken);
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddScraper();
            services.AddConsole();
            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
