using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FhatFinder.Scraper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScraper(this IServiceCollection services)
        {
            services.AddSingleton(BrowsingContext.New(Configuration.Default.WithDefaultLoader()));
            services.RegisterParsers();
            services.AddTransient<ICharBazaarScraper, CharBazaarScraper>();

            return services;
        }

        private static IServiceCollection RegisterParsers(this IServiceCollection services)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(z => z.FullName.Contains("FhatFinder.Scraper"))
                .SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                var interfaceToRegister = type.GetInterfaces()
                    .Where(x => x.Name.Contains("IParser"))
                    .Select(x => x)
                    .FirstOrDefault();

                if (interfaceToRegister != null)
                {
                    services.AddScoped(interfaceToRegister, type);
                }
            }

            return services;
        }
    }
}
