using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Pinger.Interfaces;

namespace Pinger
{
    internal class TcpPinger : IPinger
    {
        private readonly TcpClient _tcpClient;
        private readonly ISettings _settings;

        public TcpPinger(TcpClient tcpClient, ISettings settings)
        {
            _tcpClient = tcpClient;
            _settings = settings;
        }

        public string CheckStatus()
        {

            _tcpClient.Connect(_settings.Host, _settings.Port);

            byte[] data = new byte[256];
            var response = new StringBuilder();
            var stream = _tcpClient.GetStream();

            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable); // пока данные есть в потоке

            Console.WriteLine(response.ToString());

            // Закрываем потоки
            stream.Close();
            _tcpClient.Close();

            var result = "ok";
            return result;
        }
    }
}
