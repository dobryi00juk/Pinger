using System.Net.NetworkInformation;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain;
using Xunit;

namespace PingerLibTests.Domain
{
    public class IcmpPingerTests
    {
        [Fact]
        public async Task IcmpPingTest()
        {
            //Arrange
            var th = new TestHelper();
            var configuration = th.LoadConfiguration();
            var settings = new Settings(configuration);
            var ping = new Ping();

            //Act
            var icmpPinger = new IcmpPinger(ping, settings);
            var result = await icmpPinger.CheckStatusAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(typeof(string), result.GetType());
        }
    }
}