using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using PingerLib.Domain;
using PingerLib.Interfaces;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class AppTests
    {
        [Fact]
        public void StartTest()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            var logger = new Logger();
            var hosts = new List<IHost>();
            var cts = new CancellationTokenSource();
            var pingerFactory = new PingerFactory(mockServiceProvider.Object, logger);
            var app = new App(pingerFactory, logger);

            Assert.Throws<ArgumentNullException>(() => app.Start(null, cts));
            Assert.Throws<ArgumentNullException>(() => app.Start(hosts, null));
        }
    }
}
