using Microsoft.Extensions.Configuration;
using Pinger.Interfaces;

namespace Pinger.Configuration
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration) => _configuration = configuration;

        public string Host => _configuration.GetValue<string>("Host");
        public int Period => _configuration.GetValue<int>("Period") * 1000;
        public int Port => _configuration.GetValue<int>("Port");
        public string Protocol => _configuration.GetValue<string>("Protocol");
    }
}
