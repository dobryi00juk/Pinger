using System;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;


namespace Pinger
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var s = "www.google";
            var isMatch = Regex.IsMatch(s, @"^((http|ftp|https|www)://)?([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?$");
            Console.WriteLine(isMatch);
            
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            
            var app = serviceProvider.GetService<IApp>();
            var settings = serviceProvider.GetService<ISettings>();

            await app.Start();
        }
    
        private static IServiceCollection ConfigureServices()
        {
            var configuration = LoadConfiguration();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<ISettings, Settings>();
            serviceCollection.AddTransient<IPinger, HttpPinger>();
            serviceCollection.AddTransient<IPinger, IcmpPinger>();
            serviceCollection.AddTransient<IPinger, TcpPinger>();
            serviceCollection.AddTransient<HttpRequestMessage>();
            serviceCollection.AddScoped<HttpClient>();
            serviceCollection.AddTransient<Ping>();
            serviceCollection.AddSingleton<IApp, App>();
            serviceCollection.AddTransient<ILogger, Logger>();
            serviceCollection.AddScoped<SettingsValidator>();

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
