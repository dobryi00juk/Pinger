using PingerLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace PingerLib.Domain
{
    public class App : IApp
    {
        private readonly IEnumerable<IPinger> _pingerList;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public App(IEnumerable<IPinger> pingerList, ILogger logger, ISettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _pingerList = pingerList ?? throw new ArgumentNullException(nameof(pingerList));
            
            foreach (var item in _pingerList)
            {
                item.ChangeStatus += _logger.LogToFileAndConsole;
            }
        }

        public async Task Start()
        {
            if (!_settings.ValidationResult.IsValid)
            {
                HandleErrors(_settings.ValidationResult, _logger);
                return;
            }

            while (true)
            {
                foreach (var item in _pingerList)
                {
                    await item.CheckStatusAsync();
                }
                Thread.Sleep(_settings.Period * 1000);
            }
        }

        private static void HandleErrors(ValidationResult result, ILogger logger)
        {
            foreach (var item in result.Errors)
            {
                logger.LogToFile(item.ErrorMessage);
                logger.LogToFileAndConsole("Error! Check setting file.");
            }
        }
    }
}
