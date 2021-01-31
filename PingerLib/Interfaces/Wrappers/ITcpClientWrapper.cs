using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PingerLib.Interfaces.Wrappers
{
    public interface ITcpClientWrapper : IDisposable
    {
        bool Connected { get; set; }
        Task ConnectAsync(string host, int port);
        void Close();
    }
}
