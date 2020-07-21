using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class TcpPing : IPinger
    {
        private readonly TcpClient _tcpClient;
        private readonly ISettings _settings;
        public event Action<string> ChangeStatus;
        public TcpPing(TcpClient tcpClient, ISettings settings)
        {
            _tcpClient = tcpClient;
            _settings = settings;
        }

        public async Task<string> CheckStatusAsync()
        {

            await _tcpClient.ConnectAsync(_settings.Host, _settings.Port);

            byte[] data = new byte[256];
            var response = new StringBuilder();
            var stream = _tcpClient.GetStream();

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

            var result = "ok";
            return result;
        }
    }
}
