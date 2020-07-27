using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Configuration;
using Pinger.Interfaces;
using PingerLibTests;
using System.Net.NetworkInformation;

namespace Pinger.Domain.Tests
{
    [TestClass()]
    public class IcmpPingerTests
    {
        [TestMethod()]
        public void CheckStatusAsyncTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var settings = new Settings(configuration);
            var ping = new Ping();

            //Act
            var ctor = new IcmpPinger(ping, settings);
            var result = ctor.CheckStatusAsync();

            //Assert
            //Assert.AreEqual("Success", result.Result);
        }
    }
}