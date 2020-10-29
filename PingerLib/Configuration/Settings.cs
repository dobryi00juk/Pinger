using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Pinger.Interfaces;

namespace PingerLib.Configuration
{
    public class Settings : ISettings
    {
        public string Host { get; set; }
        public int Period { get; set; } 
        public int Port { get; set; }
        public Settings(IConfiguration configuration)
        {
            configuration.Bind("Settings", this);
        }
    }
}
