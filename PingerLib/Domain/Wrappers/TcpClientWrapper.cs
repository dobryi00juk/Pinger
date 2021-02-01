using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain.Wrappers
{
    public class TcpClientWrapper : ITcpClientWrapper
    {
        public bool Connected { get; set; }

        private TcpClient _tcpClient;

        public TcpClientWrapper()
        {
            _tcpClient = new TcpClient();
        }

        public TcpClientWrapper(TcpClient tcpClient)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }
        public async Task ConnectAsync(string host, int port)
        {
            if (_tcpClient.Client == null)
                _tcpClient = new TcpClient();

            await _tcpClient.ConnectAsync(host, port);
            Connected = _tcpClient.Connected;
        }

        public void Close()
        {
            _tcpClient.Close();
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
        }
    }
}
