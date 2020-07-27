using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Configuration;
using Pinger.Domain;
using PingerLibTests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pinger.Domain.Tests
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