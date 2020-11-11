using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pinger.Interfaces;
using PingerLib.Configuration;
using PingerLib.Domain;
using PingerLib.Interfaces;


namespace Pinger
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new Startup().ConfigureServices().BuildServiceProvider();
            var pingerServiceList = GetPingerServices(services).ToList();
            var settings = services.GetService<ISettings>();
            var result = await ValidateSetting(settings);

            if (!result.IsValid)
            {
                HandleErrors(services);
                return;
            }

            //var serviceList = pingerServiceList as IPinger[] ?? pingerServiceList.ToArray();

            while (true)
            {
                foreach (var item in pingerServiceList)
                {
                    await item.CheckStatusAsync();
                }
                Thread.Sleep(settings.Period * 1000);
            }
        }
    
        private static void HandleErrors(IServiceProvider services)  /*ValidationResult result, ILogger logger*/
        {
            var validationResult = services.GetService<ValidationResult>();
            var logger = services.GetService<ILogger>();

            foreach (var item in validationResult.Errors)
            {
                logger.LogToFile(item.ErrorMessage);
                logger.LogToFileAndConsole("Error! Check setting file.");
            }
        }

        private static IEnumerable<IPinger> GetPingerServices(IServiceProvider services) /*ILogger logger*/
        {
            var pingerServices = services.GetServices<IPinger>().ToList();
            var logger = services.GetService<ILogger>();

            foreach (var item in pingerServices)
                item.ChangeStatus += logger.LogToFileAndConsole;

            return pingerServices;
        }

        private static async Task<ValidationResult> ValidateSetting(ISettings settings)
        {
            var settingsValidator = new SettingsValidator();
            var validationResult = await settingsValidator.ValidateAsync(settings as Settings);

            return validationResult;
        }
    }
}
