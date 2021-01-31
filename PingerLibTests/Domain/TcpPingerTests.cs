using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Domain.Wrappers;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;
using Xunit;
using Xunit.Abstractions;


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
            var token = new CancellationToken();
            var logger = new Logger();
            var tcpClientMock = new Mock<ITcpClientWrapper>();
            tcpClientMock.Setup(x => x.ConnectAsync("123", 80))
                .Returns(Task.FromResult(default(object)));
            tcpClientMock.Setup(x => x.Connected)
                .Returns(true);

            var tcpPinger = new TcpPinger(_hosts[0], logger, tcpClientMock.Object);
            var result = await tcpPinger.GetStatusAsync(token);
            
            Assert.Equal(typeof(PingResult), result.GetType());
            Assert.Equal("Success", result.Status);
        }

        [Fact]
        public void ConstructorTest()
        {
            var logger = new Logger();

            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], null, new TcpClientWrapper()));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(null, logger, new TcpClientWrapper()));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], logger, null));
        }
    }
}