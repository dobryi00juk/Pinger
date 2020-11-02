using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Pinger.Interfaces;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class IcmpPinger : IPinger
    {
        public event Action<string> ChangeStatus;
        private readonly Ping _ping;
        private readonly ISettings _settings;
        private IPStatus OldStatus { get; set; }
        private IPStatus NewStatus { get; set; }
        public string ResponseMessage { get; set; }

        public IcmpPinger(Ping ping, ISettings settings)
        {
            _ping = ping ?? throw new ArgumentNullException(nameof(ping));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            OldStatus = IPStatus.Unknown;
        }

        

        public async Task<string> CheckStatusAsync()
        {
            var uri = _settings.Host;

            if (!uri.StartsWith("www")) uri = "www." + _settings.Host;

            try
            {
                var result = await _ping.SendPingAsync(uri, 10000);
                NewStatus = result.Status;
                ResponseMessage = CreateResponseMessage(NewStatus.ToString());

                if (NewStatus != OldStatus)
                {
                    ChangeStatus?.Invoke(ResponseMessage);
                    OldStatus = NewStatus;
                }
            }
            catch (PingException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.InnerException?.Message);
                ChangeStatus?.Invoke(_settings.Host + ": " + ResponseMessage);
            }
            catch (Exception ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }

            return ResponseMessage;
        }

        private string CreateResponseMessage(string status) =>
            "ICMP" +
            " | " + DateTime.Now +
            " | " + _settings.Host +
            " | " + status;
    }
}
