using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class TcpPinger : IPinger
    {
        private readonly Host _host;
        private readonly ILogger _logger;
        private bool _statusChanged;
        private string _newStatus;
        private string _oldStatus;

        public TcpPinger(Host host, ILogger logger)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<PingResult> GetStatusAsync(CancellationToken token)
        {
            if(token.IsCancellationRequested)
                _logger.Log("Tcp:GetStatusAsync method canceled");
            
            var status = await CheckStatusAsync(_host.HostName, token);

            return new PingResult
            {
                Protocol = "tcp",
                Date = DateTime.Now,
                Host = _host.HostName,
                Status = status,
                StatusChanged = _statusChanged
            };
        }

        private async Task<string> CheckStatusAsync(string host, CancellationToken token)
        {
            await Task.Delay(_host.Period * 1000, token);

            using var tcpClient = new TcpClient();
            try
            {
                if (token.IsCancellationRequested)
                {
                    _logger.Log("TcpPinger:CheckStatusAsync method canceled!");
                    throw new OperationCanceledException(token);
                }

                await tcpClient.ConnectAsync(host, 80);
                _newStatus = tcpClient.Connected ? "Success" : "Fail";

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
                tcpClient.Close();
            }
        }
    }
}
