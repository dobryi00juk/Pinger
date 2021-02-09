using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using PingerLib.Interfaces;
using System;
using System.Collections.Generic;

namespace PingerLib.Configuration
{
    public class Settings : ISettings
    {
        public IEnumerable<IHost> HostList { get; set; } = new List<Host>();
        public ValidationResult ValidationResult { get; set; }
        private readonly HostSettingsRules _hostRules;
        private readonly ILogger _logger;

        public Settings(
            IConfiguration configuration,
            HostSettingsRules hostRules, 
            ILogger logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hostRules = hostRules ?? throw new ArgumentNullException(nameof(hostRules));

            configuration.GetSection("Hosts").Bind(HostList, c => c.BindNonPublicProperties = true);

            ValidationResult = Validate(HostList);
        }

        private ValidationResult Validate(IEnumerable<IHost> hosts)
        {
            ValidationResult result = null;

            foreach (var item in hosts)
            {
                result = _hostRules.Validate(item as Host);
                
                if (result.IsValid)
                    continue;
                HandleErrors(result, item);
            }

            return result;
        }

        private void HandleErrors(ValidationResult result, IHost host)
        {
            foreach (var item in result.Errors)
                _logger.Log($"{host.Protocol} | {host.HostName} | {item.ErrorMessage}");
        }
    }
}
