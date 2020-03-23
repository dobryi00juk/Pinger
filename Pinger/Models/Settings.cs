using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pinger.Models
{
    public class Settings
    {
        private readonly IConfiguration _configuration;

        public Settings(IConfiguration configuration) => this._configuration = configuration;

        public string Host => _configuration.GetValue<string>("Host");
        public int Period => _configuration.GetValue<int>("Period");
        public int Port => _configuration.GetValue<int>("Port");
        public List<string> Protocols => _configuration.GetSection("Protocols").Get<List<string>>();
    }
}
