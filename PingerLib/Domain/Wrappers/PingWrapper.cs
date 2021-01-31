using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using PingerLib.Interfaces.Wrappers;

namespace PingerLib.Domain.Wrappers
{
    public class PingWrapper : IPingWrapper, IDisposable
    {
        private readonly Ping _ping;
        public IPStatus Status { get; private set; }

        public PingWrapper()
        {
            _ping = new Ping();
        }

        public async Task<IPStatus> SendPingAsync(string hostNameOrAddress, int timeout)
        {
            var result = await _ping.SendPingAsync(hostNameOrAddress, timeout);
            Status = result.Status;
            return Status;
        }

        public void Dispose()
        {
            _ping.Dispose();
        }
    }
}
