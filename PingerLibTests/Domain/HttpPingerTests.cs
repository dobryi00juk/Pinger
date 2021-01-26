using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;
using RichardSzalay.MockHttp;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class HttpPingerTests
    {
        private readonly List<IHost> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new HttpHost {HostName = "http://www.google.ru", Period = 2, Protocol = "icmp", StatusCode = 200},
        };

        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            var token = new CancellationToken();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Head, _hosts[2].HostName)
                    .Respond("application/json", "{'name' : 'TesMcGee'}");

            var client = mockHttp.ToHttpClient();
            var response = await client.GetAsync(_hosts[2].HostName, token);
            var json = await response.Content.ReadAsStringAsync(token);


            //HttpClient httpClient = new ();
            //HttpRequestMessage httpRequestMessage = new ();
            //Logger logger = new();
            //var httpPinger = new HttpPinger(httpClient, httpRequestMessage, _hosts[2] as HttpHost, logger);
            //var result = await httpPinger.GetStatusAsync(default);

            Assert.NotNull(json);
            Assert.Equal(typeof(string), json.GetType());
        }

        [Fact]
        public void ConstructorTest()
        {
            var logger = new Logger();
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage();

            Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, httpRequestMessage, _hosts[2] as HttpHost, logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, null, _hosts[2] as HttpHost, logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, httpRequestMessage, null, logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, httpRequestMessage, _hosts[2] as HttpHost, null));
        }
    }
}