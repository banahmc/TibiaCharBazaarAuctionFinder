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

        public static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            MainAsync(cts.Token).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(CancellationToken cs)
        {
            try
            {
                RegisterServices();
                IServiceScope scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<FHatFinderApp>>();
                var app = scope.ServiceProvider.GetRequiredService<IFHatFinderApp>();
                logger.LogInformation("Starting application...");
                await app.Run(cs);
            }
            finally
            {
                DisposeServices();
            }
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
