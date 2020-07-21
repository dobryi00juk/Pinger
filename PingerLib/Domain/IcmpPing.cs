using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class IcmpPing// : IPinger
    {
        public event Action<string> ChangeStatus;
        private readonly Ping _ping;
        private readonly ISettings _settings;
        private IPStatus OldStatus { get; set; }
        private IPStatus NewStatus { get; set; }

        public IcmpPing(Ping ping, ISettings settings)
        {
            _ping = ping;
            _settings = settings;
            OldStatus = IPStatus.Unknown;
        }

        public async Task<string> CheckStatusAsync()
        {
            var request = await _ping.SendPingAsync(_settings.Host, 10000) 
                ?? throw new ArgumentNullException($"_ping.Send(host)");//? атт для файла настроек,,,
            var message = "Icmp" + " | " + DateTime.Now + " | " + _settings.Host.Normalize() + " | " + request.Status;
            NewStatus = request.Status;

            if (NewStatus != OldStatus)
            {
                ChangeStatus?.Invoke(message);
                OldStatus = NewStatus;
            }

            return message;
        }
    }
}
