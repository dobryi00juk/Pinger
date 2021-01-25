using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class TcpPingerTests
    {
        private readonly List<Host> _hosts = new List<Host>
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new Host {HostName = "ya.ru", Period = 2, Protocol = "icmp"},
        };

        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            
            var setting = new Settings(configuration, _hosts, new SettingsRules(), new Logger());
            var tcpPinger = new TcpPinger(new Logger());

            //Act

            //Assert

            //Task.Run(() => tcpPinger.GetStatusAsync("www.microsoft.com", 1)).GetAwaiter().GetResult();
            //await tcpPinger.GetStatusAsync("www.microsoft.com", 1);
            //await Assert.ThrowsAsync<ArgumentNullException>(() => tcpPinger.GetStatusAsync(null, 1));
        }

        [Fact]
        public void ConstructorTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var setting = new Settings(configuration, _hosts, new SettingsRules(), new Logger());

            //Act

            //Assert
            Assert.Throws<ArgumentNullException>(() => new TcpPinger(null));
        }
    }
}