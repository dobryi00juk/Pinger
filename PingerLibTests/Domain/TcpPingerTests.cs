using PingerLib.Configuration;
using PingerLib.Domain;
using System.Threading.Tasks;
using Xunit;

namespace PingerLibTests.Domain
{
    public class TcpPingerTests
    {
        [Fact]
        public async Task CheckStatusAsyncTest()
        {
            //Arrage
            var th = new TestHelper();
            var config = th.LoadConfiguration();
            var setting = new Settings(config);

            //Act
            var tcpPinger = new TcpPinger(setting);
            var result = await tcpPinger.CheckStatusAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(typeof(string), result.GetType());
        }
    }
}