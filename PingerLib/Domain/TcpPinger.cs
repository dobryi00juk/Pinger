using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class TcpPinger : IPinger
    {
        private readonly ILogger _logger;
        public event Action<string> ErrorOccured;
        private string _statusChanged;
        private string _oldStatus;
        public TcpPinger(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task GetStatusAsync(string host, int period, CancellationToken cts)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            while (!cts.IsCancellationRequested)
            {
                var status = await CheckStatusAsync(host);
                var connection = status ? "Connected" : "Disconnected";

                if (_statusChanged == "true")
                    _logger.LogToConsole($"Tcp  | {DateTime.Now} | {host} | {connection}");

                Thread.Sleep(period * 1000);
            }
            Console.WriteLine("Tcp stop");
        }

        private async Task<bool> CheckStatusAsync(string host)
        {
            using var tcpClient = new TcpClient();
            try
            {
                await tcpClient.ConnectAsync(host, 80);
                var newStatus = tcpClient.Connected.ToString().ToLower();

                if (newStatus != _oldStatus)
                {
                    _statusChanged = "true";
                    _oldStatus = newStatus;
                }
                else
                {
                    _statusChanged = "false";
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                return false;
            }
            finally
            {
                tcpClient.Close();
            }
        }
    }
}
