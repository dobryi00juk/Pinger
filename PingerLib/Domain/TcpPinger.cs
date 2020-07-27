using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pinger.Interfaces;

namespace Pinger
{
    public class TcpPinger : IPinger
    {
        private readonly ISettings _settings;
        public event Action<string> ChangeStatus;
        private string NewStatus { get; set; }
        private string OldStatus { get; set; }
        public TcpPinger(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task CheckStatusAsync()
        {
            try
            {
                //TO DO: parse host
                using var tcpClient = new TcpClient();
                var task = Task.Run(() => tcpClient.ConnectAsync(_settings.Host, _settings.Port).Wait(1000));
                var result = await task;
                string message;

                if (result)
                {
                    NewStatus = "Success";
                    message = CreateResponseMessage(NewStatus);

                    if(NewStatus != OldStatus)
                    {
                        ChangeStatus?.Invoke(message);
                        OldStatus = NewStatus;
                    }
                }
                else
                {
                    NewStatus = "Fail";
                    message = CreateResponseMessage(NewStatus);

                    if (NewStatus != OldStatus)
                    {
                        ChangeStatus?.Invoke(message);
                        OldStatus = NewStatus;
                    }
                }
            }
            #region catch
            catch (SocketException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            catch (ObjectDisposedException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            catch (NullReferenceException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            catch (ArgumentNullException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            catch (Exception ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            #endregion
        }

        private string CreateResponseMessage(string status) =>
            "TCP" +
            " | " + DateTime.Now +
            " | " + _settings.Host.Normalize() +
            " | " + _settings.Port +
            " | " + status;
    }
}
