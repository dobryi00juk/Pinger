using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PingerLib;

namespace Pinger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILogger>();
            var icmpPinger = serviceProvider.GetService<IcmpPinger>();
            var tcpPinger = serviceProvider.GetService<TcpPinger>();
            var protocol = serviceProvider.GetService<ISettings>().Protocol;
            var htmlPinger = serviceProvider.GetService<HttpPinger>();

            icmpPinger.Notify += logger.LogToFileAndConsole;
            icmpPinger.CheckStatus();
            //switch (protocol)
            //{
            //    case "http":
            //        //logger.LogToConsole("HTTP"); 
            //        logger.LogToFileAndConsole(htmlPinger.CheckStatus());
            //        break;

            //    case "icmp":
            //        //logger.LogToConsole("ICMP");
            //        logger.LogToFileAndConsole(icmpPinger.CheckStatus());
            //        break;

            //    case "tcp":
            //        //logger.LogToConsole("TCP");
            //        logger.LogToFileAndConsole(tcpPinger.CheckStatus());
            //        break;
            //}
        }

        private static IServiceCollection ConfigureServices()
        {
            var configuration = LoadConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddTransient<HttpPinger>();
            serviceCollection.AddSingleton<ISettings, Settings>();
            serviceCollection.AddTransient<IcmpPinger>();
            serviceCollection.AddTransient<TcpPinger>();
            serviceCollection.AddTransient<HttpRequestMessage>();
            serviceCollection.AddScoped<HttpClient>();
            serviceCollection.AddScoped<TcpClient>();
            serviceCollection.AddTransient<Ping>();
            serviceCollection.AddTransient<ILogger, Logger>();

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
