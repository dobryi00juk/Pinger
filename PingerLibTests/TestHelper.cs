using System.IO;
using Microsoft.Extensions.Configuration;

namespace PingerLib.Tests
{
    public class TestHelper
    {
        public IConfiguration LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            return configuration.Build();
        }
    }
}
