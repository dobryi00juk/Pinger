using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Domain.Wrappers;
using PingerLib.Interfaces;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain
{
    public class TcpPinger : IPinger
    {
        private readonly Host _host;
        private readonly ILogger _logger;
        private readonly ITcpClientWrapper _tcpClientWrapper;
        private bool _statusChanged;
        private string _newStatus;
        private string _oldStatus;

        public TcpPinger(Host host, ILogger logger, ITcpClientWrapper tcpClientWrapper)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tcpClientWrapper = tcpClientWrapper ?? throw new ArgumentNullException(nameof(tcpClientWrapper));
        }

        public async Task<PingResult> GetStatusAsync(CancellationToken token)
        {
            if(token.IsCancellationRequested)
                _logger.Log("Tcp:GetStatusAsync method canceled");

            var status = await CheckStatusAsync(token);

            return new PingResult
            {
                Protocol = "tcp",
                Date = DateTime.Now,
                Host = _host.HostName,
                Status = status,
                StatusChanged = _statusChanged
            };
        }

        private async Task<string> CheckStatusAsync(CancellationToken token)
        {
            await Task.Delay(_host.Period * 1000, token);
            
            try
            {
                if (token.IsCancellationRequested)
                {
                    _logger.Log("TcpPinger:CheckStatusAsync method canceled!");
                    throw new OperationCanceledException(token);
                }

                using var tcpClientWrapper = new TcpClientWrapper();
                await _tcpClientWrapper.ConnectAsync(_host.HostName, 80);
                _newStatus = _tcpClientWrapper.Connected ? "Success" : "Fail";

                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                    _statusChanged = false;

                return _newStatus;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                return "Error";
            }
            finally
            {
                _tcpClientWrapper.Close();
            }
        }
    }
}
