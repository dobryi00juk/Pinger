using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class IcmpPingerTests
    {
        private readonly List<Host> _hosts = new List<Host>
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new Host {HostName = "ya.ru", Period = 2, Protocol = "icmp"},
        };

        [Fact]
        public async Task IcmpPingTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var ping = new Ping();
            var logger = new Logger();
            var icmpPinger = new IcmpPinger(ping, logger);

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => icmpPinger.GetStatusAsync(null, 2));
        }
    }
}