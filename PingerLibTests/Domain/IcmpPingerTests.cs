using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Domain.Wrappers;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;
using Xunit;
using PingReply = System.Net.NetworkInformation.PingReply;

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
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            //var pingReply = new PingerLib.Domain.Wrappers.PingReplyWrapper();
            
            var mockPingWrapper = new Mock<IPingWrapper>();
            mockPingWrapper.Setup(x => x.SendPingAsync("123", 2))
                .ReturnsAsync(IPStatus.BadRoute);
            
            //Act
            var pinger = new IcmpPinger(_hosts[1] as Host, logger, mockPingWrapper.Object);
            var result = await pinger.GetStatusAsync(token);

            //Assert
            Assert.Equal(result.Status, IPStatus.BadRoute.ToString());
            Assert.Equal(IPStatus.BadRoute.ToString(), result.Status);
            Assert.Equal((int)IPStatus.BadRoute, result.StatusCode);
        }

        [Fact]
        public void ConstructorTest()
        {
            var ping = new Ping();
            var logger = new Logger();

            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(_hosts[0] as Host, logger, null));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(null, logger, new PingWrapper()));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(_hosts[0] as Host, null, new PingWrapper()));
        }
    }
}