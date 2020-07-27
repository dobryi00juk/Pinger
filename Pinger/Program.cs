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
using FluentValidation;
using PingerLib.Configuration;

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
            var htmlPinger = serviceProvider.GetService<HttpPinger>();

            icmpPinger.ChangeStatus += logger.LogToFileAndConsole;
            htmlPinger.ChangeStatus += logger.LogToFileAndConsole;
            tcpPinger.ChangeStatus += logger.LogToFileAndConsole;

            var settings = serviceProvider.GetService<ISettings>();
            var settingsValidator = new SettingsValidator();
            var validationResult = settingsValidator.Validate(settings as Settings);
            var result = validationResult;

            //if (!(result == null && result.IsValid))
            if(!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    logger.LogToFileAndConsole(item.ErrorMessage);
                }
            }
            else
            {
                while (true)
                {
                    await htmlPinger.CheckStatusAsync();
                    await icmpPinger.CheckStatusAsync();
                    await tcpPinger.CheckStatusAsync();
                    Thread.Sleep(settings.Period * 1000);
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
