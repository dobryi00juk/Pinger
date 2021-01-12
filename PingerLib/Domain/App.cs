using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class App 
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Start(List<Host> hosts, CancellationTokenSource cts)
        {
            if (hosts == null) throw new ArgumentNullException(nameof(hosts));
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            foreach (var host in hosts)
            {
                Task.Run(() => CreatePinger(host.Protocol).GetStatusAsync(host.HostName, host.Period, cts.Token));
            }
        }

        public IPinger CreatePinger(string protocolType)
        {
            return protocolType switch
            {
                "icmp" => _serviceProvider.GetService(typeof(IcmpPinger)) as IcmpPinger,
                "tcp" => _serviceProvider.GetService(typeof(TcpPinger)) as TcpPinger,
                "http" => _serviceProvider.GetService(typeof(HttpPinger)) as HttpPinger,
                _ => throw new ArgumentException("ProtocolType Error")
            };
        }
    }
}

