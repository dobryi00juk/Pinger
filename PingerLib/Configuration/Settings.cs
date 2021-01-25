using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using PingerLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using PingerLib.Configuration.Rules;

namespace PingerLib.Configuration
{
    public class Settings 
    {
        public IEnumerable<IHost> HostList { get; set; }
        public ValidationResult ValidationResult { get; set; }

        private readonly HttpHostSettingsRules _httpHostRules;
        private readonly HostSettingsRules _hostRules;
        private readonly ILogger _logger;
        private readonly IEnumerable<Host> _hosts = new List<Host>();
        private readonly IEnumerable<HttpHost> _httpHosts= new List<HttpHost>();

        public Settings(IConfiguration configuration, HttpHostSettingsRules httpHostRules, HostSettingsRules hostRules, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpHostRules = httpHostRules ?? throw new ArgumentException(nameof(httpHostRules));
            _hostRules = hostRules ?? throw new ArgumentException(nameof(hostRules));

            configuration.GetSection("Hosts")
                .Bind(_hosts, c => c.BindNonPublicProperties = true);
                
            configuration.GetSection("HttpHosts")
                .Bind(_httpHosts, c => c.BindNonPublicProperties = true);

            HostList = _hosts.Concat(_httpHosts);

            ValidationResult = Validate(HostList);
        }

        private ValidationResult Validate(IEnumerable<IHost> hosts)
        {
            ValidationResult result = null;

            foreach (var item in hosts)
            {
                if(item is HttpHost host)
                    result = _httpHostRules.Validate(host);
                else
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
