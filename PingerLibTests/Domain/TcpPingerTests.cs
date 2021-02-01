using System;
using System.Collections.Generic;
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
    public class TcpPingerTests
    {
        private readonly ILogger _logger;
        private readonly List<Host> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new Host {HostName = "ya.ru", Period = 2, Protocol = "icmp"},
        };

        public TcpPingerTests()
        {
            _logger = new Logger();
        }

        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            var token = new CancellationToken();
            var tcpClientMock = new Mock<ITcpClientWrapper>();
            tcpClientMock
                .Setup(x => x.ConnectAsync("123", 80))
                .Returns(Task.FromResult(default(object)));
            tcpClientMock.Setup(x => x.Connected)
                .Returns(true);

            var tcpPinger = new TcpPinger(_hosts[0], _logger, tcpClientMock.Object);
            var result = await tcpPinger.GetStatusAsync(token);
            tcpClientMock.Verify(x => x.ConnectAsync(It.IsAny<string>(), It.IsAny<int>()));

            Assert.Equal(typeof(PingResult), result.GetType());
            Assert.Equal("Success", result.Status);
        }

        [Fact]
        public void ConstructorTest()
        {
            var tcpClientWrapper = new TcpClientWrapper();

            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], null, tcpClientWrapper));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(null, _logger, tcpClientWrapper));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], _logger, null));
        }
    }
}