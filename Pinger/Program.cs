using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pinger.Models;

namespace Pinger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory());

            var settings = configuration.Build();

            var serviceProvider = new ServiceCollection()
                .AddTransient<Logger>()
                .AddTransient<HttpPinger>()
                .AddTransient<IcmpPinger>()
                .AddTransient<HttpRequestMessage>()
                .AddScoped<HttpClient>()
                .AddScoped<TcpClient>()
                .AddTransient(s => new UriBuilder(settings["Host"]))
                .AddTransient(s => HttpMethod.Head)
                .AddTransient<Ping>()
                .AddTransient<Test>()
                .AddTransient<TcpPinger>()
                .AddSingleton<Settings>()
                .AddSingleton<IConfiguration>(provider => settings)
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();


            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var setting = serviceProvider.GetRequiredService<Settings>();

            var httpPinger = serviceProvider.GetRequiredService<HttpPinger>();
            var icmpPinger = serviceProvider.GetRequiredService<IcmpPinger>();
            
            logger.LogInformation("Application start");

            while (true)
            {
                try
                {
                    httpPinger.CheckStatus();
                    Thread.Sleep(setting.Period * 1000);
                    icmpPinger.CheckStatus();
                    Thread.Sleep(setting.Period * 1000);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return;
                }
            }

        }
    }
}
