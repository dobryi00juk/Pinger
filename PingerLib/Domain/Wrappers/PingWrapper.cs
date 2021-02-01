using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain.Wrappers
{
    public class PingWrapper : IPingWrapper, IDisposable
    {
        private readonly Ping _ping;

        public PingWrapper()
        {
            _ping = new Ping();
        }

        public async Task<IPStatus> SendPingAsync(string hostNameOrAddress, int timeout)
        {
            var result = await _ping.SendPingAsync(hostNameOrAddress, timeout);
            return result.Status;
        }

        public void Dispose()
        {
            _ping.Dispose();
        }
    }
}
