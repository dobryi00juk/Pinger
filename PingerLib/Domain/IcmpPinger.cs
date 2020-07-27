using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Pinger.Configuration;
using Pinger.Interfaces;

namespace Pinger.Domain
{
    public class IcmpPinger : IPinger
    {
        public event Action<string> ChangeStatus;
        private readonly Ping _ping;
        private readonly ISettings _settings;
        private IPStatus OldStatus { get; set; }
        private IPStatus NewStatus { get; set; }

        public IcmpPinger(Ping ping, ISettings settings)
        {
            _ping = ping ?? throw new ArgumentNullException(nameof(ping));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            OldStatus = IPStatus.Unknown;
        }

        public async Task CheckStatusAsync()
        {
            try
            {
                var request = await _ping.SendPingAsync(_settings.Host, 10000);
                var message = CreateResponseMessage(request.Status);
                NewStatus = request.Status;

                if (NewStatus != OldStatus)
                {
                    ChangeStatus?.Invoke(message);
                    OldStatus = NewStatus;
                }
            }
            
            #region catch

            catch (PingException ex)
            {
                ChangeStatus?.Invoke(ex.InnerException?.Message);
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
                throw;
            }
            catch(InvalidOperationException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
                throw;
            }

            #endregion
        }

        private string CreateResponseMessage(IPStatus status) =>
            "ICMP" + 
            " | " + DateTime.Now + 
            " | " + _settings.Host.Normalize() + 
            " | " + status.ToString();
    }
}
