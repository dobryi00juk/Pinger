﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PingerLib.Configuration;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain
{
    public class App 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetService<ILogger>();
        }

        public void Start(IEnumerable<IHost> hosts, CancellationTokenSource cts)
        {
            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));
            if (cts == null)
                throw new ArgumentNullException(nameof(cts));

            foreach (var host in hosts)
            {
                var task = new Task(async () =>
                {
                    await Run(host, cts.Token);
                });
                task.Start();
            }
        }

        private async Task Run(IHost host, CancellationToken token)
        {
            IPinger pinger = null;

            try
            {
                pinger = CreatePinger(host);

                while (true)
                {
                    var result  = await pinger.GetStatusAsync(token);
                    
                    if(result.StatusChanged)
                        _logger.Log(CreateResponseMessage(result));
                }
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == token)
            {
                if (pinger != null) _logger.Log(pinger.GetType() + " | " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }
        }

        private static string CreateResponseMessage(PingResult result)
        {
            return result.StatusCode == null
                ? (_ = $"{result.Protocol} | {result.Date} | {result.Host} | {result.Status}")
                : $"{result.Protocol} | {result.Date} | {result.Host} | {result.Status} | Status code: {result.StatusCode}";
        }

        private IPinger CreatePinger(IHost host)
        {
            return host.Protocol switch
            {
                "icmp" => new IcmpPinger((Host) host, _logger, _serviceProvider.GetService<IPingWrapper>()),                                                               
                
                "tcp" => new TcpPinger((Host) host, _logger, _serviceProvider.GetService<ITcpClientWrapper>()),                                                                                                      
                
                "http" => new HttpPinger(
                    _serviceProvider.GetService<HttpClient>(), 
                    _serviceProvider.GetService<HttpRequestMessage>(),
                    (HttpHost) host, _logger),  
                
                _ => throw new ArgumentException("ProtocolType Error")
            };
        }
    }
}

