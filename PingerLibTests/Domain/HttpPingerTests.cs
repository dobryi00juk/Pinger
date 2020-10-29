using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Domain;
using PingerLib.Configuration;

namespace PingerLibTests.Domain
{
    [TestClass()]
    public class HttpPingerTests
    {
        //[TestMethod()]
        //public void HttpPingerTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void CheckStatusAsyncTest()
        {
            //arrage
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var setting = new Settings(configuration);
            var httpClient = new HttpClient();
            
            //act
            var httpPinger = new HttpPinger(httpClient, setting, new HttpRequestMessage());
            var result = httpPinger.CheckStatusAsync();

            //assert
            //Assert.AreEqual("OK", result.Result);
        }
    }
}