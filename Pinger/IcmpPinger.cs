using Microsoft.Extensions.Logging;
using Pinger.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Pinger
{
    internal class IcmpPinger : IPinger
    {
        private readonly Ping _ping;
        private readonly Logger _logger;
        private readonly Settings _settings;
        private readonly UriBuilder _uriBuilder;

        public IcmpPinger(Ping ping, Logger logger, Settings settings, UriBuilder uriBuilder)
        {
            _ping = ping;
            _logger = logger;
            _settings = settings;
            _uriBuilder = uriBuilder;
        }

        public void CheckStatus()
        {
            var host = "www." + _uriBuilder.Host;
            
            try
            {
                PingReply request = _ping.Send(host);
                var message = DateTime.Now + " | " + host.Normalize() + " | " + request.Status.ToString();

                _logger.LogToFileAndConsole(message);

            }
            catch (UriFormatException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
            catch (PingException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message + "( Incorrect ulr?)");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
        }
    }
}
