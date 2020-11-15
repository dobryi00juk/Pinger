using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Configuration
{
    public class Settings : ISettings
    {
        public string Host { get; set; }
        public int Period { get; set; } 
        public int Port { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public Settings(IConfiguration configuration, SettingsValidator rules)
        {
            configuration.Bind("Settings", this);
            ValidationResult = rules.Validate(this);
        }
    }
}
