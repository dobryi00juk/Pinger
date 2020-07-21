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
using System;
using System.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;

namespace Pinger
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var host = ConfigurationManager.AppSettings["Host"];
            Console.WriteLine(host);
            Console.WriteLine(123123123);
            
            //var logger = serviceProvider.GetService<ILogger>();
            //var icmpPinger = serviceProvider.GetService<IcmpPinger>();
            //var tcpPinger = serviceProvider.GetService<IcmpPinger>();
            //var period = serviceProvider.GetService<ISettings>().Period;
            //var htmlPinger = serviceProvider.GetService<HttpPinger>();

            //icmpPinger.ChangeStatus += logger.LogToFileAndConsole;
            //htmlPinger.ChangeStatus += logger.LogToFileAndConsole;
            //tcpPinger.ChangeStatus += logger.LogToFileAndConsole;
            
            //while (true)
            //{
            //    try
            //    {
            //        await htmlPinger.CheckStatusAsync();
            //        await icmpPinger.CheckStatusAsync();
            //        Thread.Sleep(period);
            //    }
            //    catch (Exception)
            //    {
            //        icmpPinger.ChangeStatus -= logger.LogToFileAndConsole;
            //        htmlPinger.ChangeStatus -= logger.LogToFileAndConsole;
            //        tcpPinger.ChangeStatus -= logger.LogToFileAndConsole;
            //    }
            //}
        }

        private static IServiceCollection ConfigureServices()
        {
            var configuration = LoadConfiguration();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddTransient<HttpPing>();
            serviceCollection.AddSingleton<ISettings, Settings>();
            serviceCollection.AddTransient<IcmpPing>();
            serviceCollection.AddTransient<TcpPing>();
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
