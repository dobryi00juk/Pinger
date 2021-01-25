using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class IcmpPinger : IPinger
    {
        private readonly Ping _ping;
        private readonly Host _host;
        private readonly ILogger _logger;
        private bool _statusChanged;
        public IPStatus Status;
        private int _oldStatus = -2;
        private int _newStatus;

        public IcmpPinger(Ping ping, Host host, ILogger logger)
        {
            Status = IPStatus.Unknown;
            _ping = ping ?? throw new ArgumentNullException(nameof(ping));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PingResult> GetStatusAsync(CancellationToken token)
        {
            var status = await CheckStatusAsync(_host.HostName, token);

            if (token.IsCancellationRequested)
                _logger.Log("IcmpPinger:GetStatusAsync method canceled");

            return new PingResult
            {
                Protocol = "icmp",
                Date = DateTime.Now,
                Host = _host.HostName,
                Status = status.ToString(),
                StatusCode = (int)status,
                StatusChanged = _statusChanged
            };
        }

        private async Task<IPStatus> CheckStatusAsync(string host, CancellationToken token)
        {
            try
            {
                await Task.Delay(_host.Period * 1000, token);

                if (token.IsCancellationRequested)
                {
                    _logger.Log("IcmpPinger:CheckStatusAsync method canceled!");
                    throw new OperationCanceledException(token);
                }

                var result = await _ping.SendPingAsync(host, 10000);
                _newStatus = (int) result.Status;

                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                    _statusChanged = false;

                return result.Status;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                throw;
            }
        }
    }
}
