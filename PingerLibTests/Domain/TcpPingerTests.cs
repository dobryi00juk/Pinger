using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class TcpPingerTests
    {
        private readonly List<Host> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new Host {HostName = "ya.ru", Period = 2, Protocol = "icmp"},
        };

        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            var logger = new Logger();
            var tcpPinger = new TcpPinger(_hosts[0], logger);
            var token = new CancellationToken();

            var result = await tcpPinger.GetStatusAsync(token);

            Assert.Equal(typeof(PingResult), result.GetType());
        }

        [Fact]
        public void ConstructorTest()
        {
            var logger = new Logger();

            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], null));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(null, logger));
        }
    }
}