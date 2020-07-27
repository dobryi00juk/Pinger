using Pinger.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pinger.Configuration
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;
        
        public string Host { get; set; }
        public int Period { get; set; }
        public int Port { get; set; }
        public Settings(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.Bind("Settings", this);
        }
    }
}
