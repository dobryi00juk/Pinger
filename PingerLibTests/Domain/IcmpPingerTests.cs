using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Domain;
using PingerLib.Configuration;
using PingerLib.Domain;

namespace PingerLibTests.Domain
{
    [TestClass]
    public class IcmpPingerTests
    {
        [TestMethod]
        public void CreateResponseMessageReturnStringTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var settings = new Settings(configuration);
            var ping = new Ping();

            var expected = typeof(string);

            //Act
            var ctor = new IcmpPinger(ping, settings);
            var actual = ctor.CreateResponseMessage("test string");

            //Assert
            Assert.AreEqual(expected, actual.GetType());
        }

        [TestMethod]
        public async Task ChangeStatusAsyncTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var settings = new Settings(configuration);
            var ping = new Ping();

            var expected = typeof(string);

            var ctor = new IcmpPinger(ping, settings);
            var actual = await ctor.CheckStatusAsync();

            Assert.AreEqual(expected, actual.GetType());

        }
    }
}