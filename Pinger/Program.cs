using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;


namespace Pinger
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var settings = serviceProvider.GetService<Settings>();

            if (!settings.ValidationResult.IsValid)
                return;

            var app = serviceProvider.GetService<App>();
            app.Start(settings.HostList);
            Console.ReadKey();
        }
    
        private static IServiceCollection ConfigureServices()
        {
            var configuration = LoadConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<List<Host>>();
            serviceCollection.AddTransient<HttpRequestMessage>();
            serviceCollection.AddScoped<HttpClient>();
            serviceCollection.AddTransient<Ping>();
            serviceCollection.AddTransient<TcpPinger>();
            serviceCollection.AddTransient<HttpPinger>();
            serviceCollection.AddTransient<IcmpPinger>();
            serviceCollection.AddTransient<PingReply>();
            serviceCollection.AddSingleton<App>();
            serviceCollection.AddTransient<ILogger, Logger>();
            serviceCollection.AddSingleton<Settings>();
            serviceCollection.AddScoped<SettingsRules>();

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
