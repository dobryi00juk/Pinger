using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class IcmpPinger : IPinger
    {
        private readonly Ping _ping;
        private readonly ILogger _logger;
        public event Action<string> ErrorOccured;
        private bool _statusChanged;
        public IPStatus Status;
        private int _oldStatus = -2;
        private int _newStatus;

        public IcmpPinger(Ping ping, ILogger logger)
        {
            Status = IPStatus.Unknown;
            _ping = ping ?? throw new ArgumentNullException(nameof(ping));
            _logger = logger;
        }

        public async Task GetStatusAsync(string host, int period)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            while (true)
            {
                var status = await CheckStatusAsync(host);

                if (_statusChanged)
                    _logger.LogToConsole($"Icmp | {DateTime.Now} | {host} | {status}");
                
                Thread.Sleep(period * 1000);
            }
        }

        private async Task<IPStatus> CheckStatusAsync(string host)
        {
            try
            {
                var result = await _ping.SendPingAsync(host, 10000);
                _newStatus = (int) result.Status;

                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                {
                    _statusChanged = false;
                }

                return result.Status;
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                return IPStatus.BadOption;
            }
        }
    }
}
