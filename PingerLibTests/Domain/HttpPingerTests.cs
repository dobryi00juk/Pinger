using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class HttpPingerTests
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
            var logger = new Logger();
            var rules = new SettingsRules();
            var setting = new Settings(configuration, _hosts, rules, logger);
            var httpRequestMessage = new HttpRequestMessage();
            var httpClient = new HttpClient();

            //Act
            var httpPinger = new HttpPinger(httpClient, httpRequestMessage, logger);

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => httpPinger.GetStatusAsync(null, 2));
        }

        [Fact]
        public void ConstructorTest()
        {
            //HttpClient httpClient, ISettings settings, HttpRequestMessage httpRequestMessage
            var th = new TestHelper();
            var logger = new Logger();
            var configuration = th.LoadConfiguration();
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, httpRequestMessage, logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, null, logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, httpRequestMessage, null));
        }
    }
}