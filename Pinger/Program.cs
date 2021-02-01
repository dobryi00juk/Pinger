using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerLib.Configuration;
using PingerLib.Configuration.Rules;
using PingerLib.Domain;
using PingerLib.Domain.Wrappers;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;

namespace Pinger
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Press <enter> to stop");

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var settings = serviceProvider.GetService<Settings>();
            var cts = new CancellationTokenSource();

            if (!settings.ValidationResult.IsValid)
                return;

            var app = serviceProvider.GetService<App>();
            app.Start(settings.HostList, cts);

            if (Console.ReadKey().Key == ConsoleKey.Enter)
                cts.Cancel();

            Console.ReadKey();
        }
    
        private static IServiceCollection ConfigureServices()
        {
            var configuration = LoadConfiguration();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddTransient<HttpHost>();
            serviceCollection.AddTransient<Host>();
            serviceCollection.AddTransient<HttpRequestMessage>();
            serviceCollection.AddTransient<HttpClient>();
            serviceCollection.AddSingleton<App>();
            serviceCollection.AddSingleton<ILogger, Logger>();
            serviceCollection.AddTransient<ITcpClientWrapper, TcpClientWrapper>();
            serviceCollection.AddTransient<IPingWrapper, PingWrapper>();
            serviceCollection.AddSingleton<Settings>();
            serviceCollection.AddScoped<HttpHostSettingsRules>();
            serviceCollection.AddScoped<HostSettingsRules>();

            return serviceCollection;
        }

        private static IConfiguration LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            return configuration.Build();
        }
    }
}
