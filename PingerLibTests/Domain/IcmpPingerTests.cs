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
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new HttpHost() {HostName = "ya.ru", Period = 2, Protocol = "icmp", StatusCode = 200},
        };

        [Fact]
        public async Task<PingResult> IcmpPingTest()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => icmpPinger.GetStatusAsync(null, 2, ct));
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