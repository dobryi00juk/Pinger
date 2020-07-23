using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Pinger.Interfaces;

namespace Pinger
{
    public class TcpPinger : IPinger
    {
        private readonly TcpClient _tcpClient;
        private readonly ISettings _settings;
        public event Action<string> ChangeStatus;
        public TcpPinger(TcpClient tcpClient, ISettings settings)
        {
            _tcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<string> CheckStatusAsync()
        {

            await _tcpClient.ConnectAsync(_settings.Host, _settings.Port);
            //var ip = IPAddress.Parse("127.0.0.1");
            //await _tcpClient.ConnectAsync(ip, 8888);

            byte[] data = new byte[_tcpClient.ReceiveBufferSize];
            var stream = _tcpClient.GetStream();
            var response = new StringBuilder();

            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable); // пока данные есть в потоке

            ChangeStatus?.Invoke(response.ToString());

            // Закрываем потоки
            stream.Close();
            _tcpClient.Close();

            var result = "OK";
            return result;
        }
    }
}
