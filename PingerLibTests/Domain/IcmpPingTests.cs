using NUnit.Framework;
using PingerLib.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Extensions.Configuration;
using Moq;
using Pinger.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Domain.Tests
{
    [TestFixture]
    public class IcmpPingTests
    {
        private IcmpPing _icmpPing;
        private Ping _ping;
        private ISettings _settings;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            var mockConfSection = new Mock<IConfiguration>()
                .SetupGet(m => m[It.Is<string>(s => s == "Host")]);

            _ping = new Ping();
            _settings = new Settings(_configuration);
            _icmpPing = new IcmpPing(_ping, _settings);
        }
        [Test]
        public void IcmpPingTest()
        {
            Assert.Fail();
        }

        [Test]
        public void CheckStatusAsyncTest()
        {
            Assert.Fail();
        }
    }
}