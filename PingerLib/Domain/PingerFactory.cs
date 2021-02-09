using Microsoft.Extensions.DependencyInjection;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;
using System;
using System.Net.Http;

namespace PingerLib.Domain
{
    public class PingerFactory : IPingerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public PingerFactory(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public IPinger CreatePinger(IHost host)
        {
            return host.Protocol switch
            {
                "icmp" => new IcmpPinger(host, _logger, _serviceProvider.GetService<IPingWrapper>()),

                "tcp" => new TcpPinger(host, _logger, _serviceProvider.GetService<ITcpClientWrapper>()),

                "http" => new HttpPinger(_serviceProvider.GetService<HttpClient>(), 
                    _serviceProvider.GetService<HttpRequestMessage>(), host, _logger),

                _ => throw new ArgumentException("ProtocolType Error")
            };
        }
    }
}
