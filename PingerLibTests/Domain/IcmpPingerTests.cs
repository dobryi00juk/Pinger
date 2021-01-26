using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class IcmpPingerTests
    {
        private readonly List<IHost> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "icmp"},
            new HttpHost() {HostName = "ya.ru", Period = 2, Protocol = "http", StatusCode = 200},
        };

        [Fact]
        public async Task IcmpPingTest()
        {
            //Arrange
            var ping = new Ping();
            var logger = new Logger();
            var icmpPinger = new IcmpPinger(ping, _hosts[1] as Host, logger);
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            //Act
            var result = await icmpPinger.GetStatusAsync(token);

            //Assert
            Assert.Equal(typeof(PingResult), result.GetType());
        }

        [Fact]
        public void ConstructorTest()
        {
            var ping = new Ping();
            var logger = new Logger();

            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(null, _hosts[0] as Host, logger));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(ping, null, logger));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(ping, _hosts[1] as Host, null));
        }
    }
}