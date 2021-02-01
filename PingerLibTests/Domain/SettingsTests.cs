using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using PingerLib.Configuration;
using PingerLib.Configuration.Rules;
using PingerLib.Domain;
using Xunit;

namespace PingerLib.Tests.Domain
{
    public class SettingsTests
    {
        private readonly Logger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpHostSettingsRules _httpHostRules;
        private readonly HostSettingsRules _hostRules;

        private static IConfiguration LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            return configuration.Build();
        }
        public SettingsTests()
        {
            _hostRules = new HostSettingsRules();
            _configuration = LoadConfiguration();
            _httpHostRules = new HttpHostSettingsRules();
            _logger = new Logger();
        }

        [Fact]
        public void SettingsConstructorTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Settings(null, _httpHostRules, _hostRules, _logger));

            Assert.Throws<ArgumentNullException>(
                () => new Settings(_configuration, null, _hostRules, _logger));

            Assert.Throws<ArgumentNullException>(
                () => new Settings(_configuration, _httpHostRules, null, _logger));

            Assert.Throws<ArgumentNullException>(
                () => new Settings(_configuration, _httpHostRules, _hostRules, null));
        }
    }
}
