using System;
using PingerLib.Configuration;
using PingerLib.Domain;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PingerLibTests.Domain
{
    public class HttpPingerTests
    {
        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            //arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var setting = new Settings(configuration);
            var httpClient = new HttpClient();
            
            //act
            var httpPinger = new HttpPinger(httpClient, setting, new HttpRequestMessage());
            var result = await httpPinger.CheckStatusAsync();

            //assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(typeof(string), result.GetType());
        }

        [Fact]
        public void HttpPingerConstructorTest()
        {
            var httpClient = new HttpClient();
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var setting = new Settings(configuration);
            var httpRequestMessage = new HttpRequestMessage();

            Assert.Throws<ArgumentNullException>(() => new HttpPinger(null, setting, httpRequestMessage));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, null, httpRequestMessage));
            Assert.Throws<ArgumentNullException>(() => new HttpPinger(httpClient, setting, null));
        }
    }
}