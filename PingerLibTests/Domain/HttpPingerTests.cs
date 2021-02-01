using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class HttpPingerTests
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly List<IHost> _hosts = new()
        {
            new Host {HostName = "www.microsoft.com", Period = 3, Protocol = "tcp"},
            new Host {HostName = "google.com", Period = 1, Protocol = "http"},
            new HttpHost {HostName = "http://www.google.ru", Period = 2, Protocol = "icmp", StatusCode = 200},
        };

        public HttpPingerTests()
        {
            _logger = new Logger();
            _httpClient = new HttpClient();
            _httpRequestMessage = new HttpRequestMessage();
        }

        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            var token = new CancellationToken();
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Locked,
            };
            
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            
            var httpClient = new HttpClient(handlerMock.Object);
            var httpPinger = new HttpPinger(httpClient, _httpRequestMessage, _hosts[2] as HttpHost, _logger);
            var result = await httpPinger.GetStatusAsync(token);
            
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Head),
                ItExpr.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.Equal(typeof(PingResult), result.GetType());
        }

        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, _httpRequestMessage, _hosts[2] as HttpHost, _logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(_httpClient, null, _hosts[2] as HttpHost, _logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(_httpClient, _httpRequestMessage, null, _logger));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(_httpClient, _httpRequestMessage, _hosts[2] as HttpHost, null));
        }
    }
}