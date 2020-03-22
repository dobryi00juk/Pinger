using Microsoft.Extensions.Configuration;
using Pinger.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Pinger
{
    class TcpPinger
    {
        private readonly TcpClient tcpClient;
        private readonly Settings settings;

        public TcpPinger(TcpClient tcpClient, Settings settings)
        {
            this.tcpClient = tcpClient;
            this.settings = settings;
        }

        //public void CheckStatus()
        //{
        //    var port = settings.Port;
        //    var host = settings.Host;

        //    tcpClient.Connect("192.168.5.1", 443);
            
        //    using NetworkStream netWorkStream = tcpClient.GetStream();
        //    netWorkStream.ReadTimeout = 2000;

        //    using var writer = new StreamWriter(netWorkStream);

        //    var message = "HEAD / HTTP/1.1\r\nHost: 192.168.5.1\r\nUser-Agent: C# program\r\n"
        //        + "Connection: close\r\nAccept: text/html\r\n\r\n";

        //    Console.WriteLine(message);

        //    using var reader = new StreamReader(netWorkStream, Encoding.UTF8);
        //    byte[] bytes = Encoding.UTF8.GetBytes(message);

        //    netWorkStream.Write(bytes, 0, bytes.Length);

        //    Console.WriteLine(reader.ReadToEnd());

        //    tcpClient.Close();
        //}

        public void CheckStatus()
        {
            var server = settings.Host;
            var port = settings.Port;

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(server, port);

                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = client.GetStream();

                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable); // пока данные есть в потоке

                Console.WriteLine(response.ToString());

                // Закрываем потоки
                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            Console.WriteLine("Запрос завершен...");
            Console.Read();
        }
    }
}
