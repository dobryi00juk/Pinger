using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using PingerLib.Interfaces;
using System;
using System.Collections.Generic;

namespace PingerLib.Configuration
{
    public class Settings 
    {
        private readonly ILogger _logger;
        public List<Host> HostList;
        public ValidationResult ValidationResult { get; set; }
        public Settings(IConfiguration configuration, List<Host> hosts, SettingsRules rules, ILogger logger)
        {
            HostList = hosts;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            configuration.GetSection("Settings").Bind(HostList, c => c.BindNonPublicProperties = true);

            foreach (var item in HostList)
            {
                ValidationResult = rules.Validate(item);

                if (ValidationResult.IsValid) continue;
                HandleErrors(ValidationResult);
                return;
            }
        }

        private void HandleErrors(ValidationResult result)
        {
            foreach (var item in result.Errors)
                _logger.LogToConsole(item.ErrorMessage);
        }
    }
}
