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
        }

        [Fact]
        public void ConstructorTest()
        {
            var logger = new Logger();

            Assert.Throws<ArgumentNullException>(() => new TcpPinger(_hosts[0], null, new TcpClient()));
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(null, logger, new TcpClient()));
            //todo
        }
    }
}