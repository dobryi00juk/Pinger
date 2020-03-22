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
using Pinger.Models;

namespace Pinger
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .SetBasePath(Directory.GetCurrentDirectory());

            var settings = configuration.Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
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
                .BuildServiceProvider();


            //var icmpPinger = serviceProvider.GetRequiredService<IcmpPinger>();
            //icmpPinger.CheckStatus();

            var httpPinger = serviceProvider.GetRequiredService<HttpPinger>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Start");
            
            var message = httpPinger.CheckStatus();
            logger.LogInformation(message);
            logger.LogInformation("Stop");
            
            
            //var tcpPinger = serviceProvider.GetRequiredService<TcpPinger>();
            //tcpPinger.CheckStatus();
        }

        //private static void ConfigureServices(IServiceCollection serviceCollection)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", false)
        //        .Build();
        //    // Add access to generic IConfigurationRoot
        //    serviceCollection.AddSingleton(configuration);

        //    serviceCollection.AddLogging(loggingBuilder =>
        //    {
        //        loggingBuilder.AddConsole();
        //    });

        //    // Add services
        //    serviceCollection.AddTransient<Test>();
        //    serviceCollection.AddSingleton<HttpClient>();
        //    serviceCollection.AddTransient<HttpRequestMessage>();
        //    serviceCollection.AddTransient(s => HttpMethod.Head);
        //    serviceCollection.AddTransient(s => new Uri("https://translate.google.com"));
        //    serviceCollection.AddTransient<Ping>();
        //    serviceCollection.AddTransient<HttpPinger>();
        //    serviceCollection.AddTransient<IcmpPinger>()
        //        .BuildServiceProvider();
        //}
    }
}
