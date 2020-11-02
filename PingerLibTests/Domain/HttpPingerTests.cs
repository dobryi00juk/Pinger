using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PingerLib.Configuration;
using PingerLib.Domain;

namespace PingerLibTests.Domain
{
    [TestClass]
    public class HttpPingerTests
    {
        //[TestMethod()]
        //public void HttpPingerTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod]
        public async Task CheckStatusAsyncTestExpectString()
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
            Assert.AreEqual(result.GetType(), typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task CheckStatusAsyncTestExpectException()
        {
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var setting = new Settings(configuration);
            var httpClient = new HttpClient();

            //act
            var httpPinger = new HttpPinger(httpClient, setting, new HttpRequestMessage());
            var result = await httpPinger.CheckStatusAsync();

            //assert
            Assert.AreEqual(typeof(HttpRequestException), result);
        }
    }
}