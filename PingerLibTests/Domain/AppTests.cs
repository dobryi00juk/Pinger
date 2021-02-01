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
            var mock = new Mock<IServiceProvider>();
            var app = new App(mock.Object);
            var hosts = new List<IHost>();
            var cts = new CancellationTokenSource();

            Assert.Throws<ArgumentNullException>(() => app.Start(null, cts));
            Assert.Throws<ArgumentNullException>(() => app.Start(hosts, null));
        }
    }
}
