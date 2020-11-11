using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pinger.Interfaces;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Pinger
{
    internal class Startup 
    {
        public IServiceCollection ConfigureServices()
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
            serviceCollection.AddScoped<TcpClient>();
            serviceCollection.AddTransient<Ping>();
            serviceCollection.AddTransient<PingerLib.Interfaces.ILogger, Logger>();
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
