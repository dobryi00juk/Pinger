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
    class IcmpPinger : IPinger
    {
        private readonly Ping ping;
        private readonly ILogger logger;
        private readonly Settings settings;
        private readonly UriBuilder uriBuilder;

        public IcmpPinger(Ping ping, ILogger<IcmpPinger> logger, Settings settings, UriBuilder uriBuilder)
        {
            this.ping = ping;
            this.logger = logger;
            this.settings = settings;
            this.uriBuilder = uriBuilder;
        }

        public void CheckStatus()
        {
            string host = uriBuilder.Host;

            try
            {
                PingReply request = ping.Send(host);
                //string message = DateTime.Now + " | " + host.Normalize() + " | " + request.Status.ToString();
                Console.WriteLine(DateTime.Now + " | " + host.Normalize() + " | " + request.Status.ToString());

                //logger.LogInformation(message.ToUpper());
                
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (PingException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("wrong URL!!!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
