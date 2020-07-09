using System;
using System.Net.NetworkInformation;
using System.Threading;
using Microsoft.Extensions.Logging;
using Pinger.Configuration;
using Pinger.Interfaces;

namespace Pinger.Domain
{
    internal class IcmpPinger //: IPinger
    {
        private readonly Ping _ping;
        private readonly ISettings _settings;
        public IPStatus oldStatus { get; private set; }
        public IPStatus newStatus { get; private set; }

        public IcmpPinger(Ping ping, ISettings settings)
        {
            _ping = ping;
            _settings = settings;
        }

        public string CheckStatus()
        {
            var count = 0;
            oldStatus = IPStatus.Success;

            while (true)
            {
                var request = _ping.Send(_settings.Host) ?? throw new ArgumentNullException($"_ping.Send(host)");
                
                newStatus = request.Status;


                switch (count)
                {
                    case 5:
                    case 10:
                    case 15:
                    case 20:
                    case 25:
                        newStatus = IPStatus.TimedOut;
                        break;
                }

                var message = DateTime.Now + " | " + _settings.Host.Normalize() + " | " + newStatus;//request.Status;

                
                if (newStatus != oldStatus)
                {
                    Notify?.Invoke(message);
                }

                oldStatus = newStatus;
                count++;


                //Thread.Sleep(2000);
            }
        }

        internal delegate void ChangeHandler(string message);

        public event ChangeHandler Notify;
    }
}
