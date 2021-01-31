using System.Net.Sockets;
using System.Threading.Tasks;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain.Wrappers
{
    public class TcpClientWrapper : ITcpClientWrapper
    {
        private TcpClient _tcpClient;
        public bool Connected { get; set; }
        //public TcpClientWrapper()
        //{
        //    _tcpClient = new TcpClient();
        //}

        public async Task ConnectAsync(string host, int port)
        {
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
