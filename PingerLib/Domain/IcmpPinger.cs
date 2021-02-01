using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain
{
    public class IcmpPinger : IPinger
    {
        private readonly Host _host;
        private readonly ILogger _logger;
        private readonly IPingWrapper _pingWrapper;
        private bool _statusChanged;
        public IPStatus Status;
        private int _oldStatus = -2;
        private int _newStatus;

        public IcmpPinger(Host host, ILogger logger, IPingWrapper pingWrapper)
        {
            Status = IPStatus.Unknown;
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pingWrapper = pingWrapper ?? throw new ArgumentNullException(nameof(pingWrapper));
        }

        public async Task<PingResult> GetStatusAsync(CancellationToken token)
        {
            var status = await CheckStatusAsync(token);

            if (token.IsCancellationRequested)
                _logger.Log("IcmpPinger:GetStatusAsync method canceled");

            return new PingResult
            {
                Protocol = "icmp",
                Date = DateTime.Now,
                Host = _host.HostName,
                Status = status.ToString(),
                StatusChanged = _statusChanged
            };
        }

        private async Task<IPStatus> CheckStatusAsync(CancellationToken token)
        {
            try
            {
                await Task.Delay(_host.Period * 1000, token);

                if (token.IsCancellationRequested)
                {
                    _logger.Log("IcmpPinger:CheckStatusAsync method canceled!");
                    throw new OperationCanceledException(token);
                }

                var result = await _pingWrapper.SendPingAsync(_host.HostName, 5000);
                _newStatus = (int)result;
                 
                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                    _statusChanged = false;

                return result;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                throw;
            }
        }
    }
}
