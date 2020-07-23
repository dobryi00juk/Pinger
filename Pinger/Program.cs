using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pinger.Configuration;
using Pinger.Domain;
using Pinger.Interfaces;
using System;
using System.Configuration;

namespace Pinger
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger>();
            var icmpPinger = serviceProvider.GetService<IcmpPinger>();
            var tcpPinger = serviceProvider.GetService<TcpPinger>();
            var period = serviceProvider.GetService<ISettings>().Period;
            var htmlPinger = serviceProvider.GetService<HttpPinger>();

            icmpPinger.ChangeStatus += logger.LogToFileAndConsole;
            htmlPinger.ChangeStatus += logger.LogToFileAndConsole;
            tcpPinger.ChangeStatus += logger.LogToFileAndConsole;

            //var result = await tcpPinger.CheckStatusAsync();
            //Console.WriteLine(result);
            while (true)
            {
                try
                {
                    await htmlPinger.CheckStatusAsync();
                    await icmpPinger.CheckStatusAsync();
                    await tcpPinger.CheckStatusAsync();
                    Thread.Sleep(period);
                }
                catch (Exception)
                {
                    icmpPinger.ChangeStatus -= logger.LogToFileAndConsole;
                    htmlPinger.ChangeStatus -= logger.LogToFileAndConsole;
                    tcpPinger.ChangeStatus -= logger.LogToFileAndConsole;
                }
            }
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
