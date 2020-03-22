using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pinger.Models
{
    class Settings
    {
        private readonly IConfiguration configuration;

        public Settings(IConfiguration configuration) => this.configuration = configuration;

        public string Host => configuration.GetValue<string>("Host");
        public int Period => configuration.GetValue<int>("Period");
        public int Port => configuration.GetValue<int>("Port");
        public List<string> Protocols => configuration.GetSection("Protocols").Get<List<string>>();
    }
}
