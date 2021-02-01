using System;
using System.Collections.Generic;
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

namespace PingerLib.Tests.Domain
{
    public class IcmpPingerTests
    {
        private readonly ILogger _logger;
        private readonly PingWrapper _pingWrapper;
        private readonly CancellationToken _token;
        private readonly List<IHost> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "ya.com", Period = 1, Protocol = "icmp"},
            new HttpHost {HostName = "ya.ru", Period = 2, Protocol = "http", StatusCode = 200},
        };

        public IcmpPingerTests()
        {
            _logger = new Logger();
            _pingWrapper = new PingWrapper();
            _token = new CancellationToken();
        }

        [Fact]
        public async Task IcmpPingTest()
        {
            //Arrange
            const IPStatus response = IPStatus.BadDestination;
            
            var mockPingWrapper = new Mock<IPingWrapper>();
            mockPingWrapper.
                Setup(x => x.SendPingAsync(
                    It.IsAny<string>(), 
                    It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var pinger = new IcmpPinger(_hosts[1] as Host, _logger, mockPingWrapper.Object);
            var result = await pinger.GetStatusAsync(_token);
            mockPingWrapper.Verify(x => x.SendPingAsync(It.IsAny<string>(), It.IsAny<int>()), "beda");

            //Assert
            Assert.Equal(typeof(PingResult), result.GetType());
            Assert.Equal(IPStatus.BadDestination.ToString(), result.Status);
        }

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(_hosts[0] as Host, _logger, null));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(null, _logger, _pingWrapper));
            Assert.Throws<ArgumentNullException>(() => new IcmpPinger(_hosts[0] as Host, null, _pingWrapper));
        }
    }
}