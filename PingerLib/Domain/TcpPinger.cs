using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pinger.Interfaces;
using PingerLib.Interfaces;

namespace Pinger
{
    public class TcpPinger : IPinger
    {
        private readonly ISettings _settings;
        public event Action<string> ChangeStatus;
        private string NewStatus { get; set; }
        private string OldStatus { get; set; }
        public string ResponseMessage { get; set; }
        public TcpPinger(ISettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }


        public async Task<string> CheckStatusAsync()
        {
            using var tcpClient = new TcpClient();
            try
            {
                var task = Task.Run(() => tcpClient.ConnectAsync(_settings.Host, _settings.Port).Wait(1000));
                var result = await task;

                if (result)
                {
                    NewStatus = "Success";
                    ResponseMessage = CreateResponseMessage(NewStatus);

                    if (NewStatus != OldStatus)
                    {
                        ChangeStatus?.Invoke(ResponseMessage);
                        OldStatus = NewStatus;
                    }

                }
                else
                {
                    NewStatus = "Fail";
                    ResponseMessage = CreateResponseMessage(NewStatus);

                    if (NewStatus != OldStatus)
                    {
                        ChangeStatus?.Invoke(ResponseMessage);
                        OldStatus = NewStatus;
                    }

                }
            }

            #region catch

            catch (SocketException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (ObjectDisposedException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (NullReferenceException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (ArgumentNullException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (AggregateException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (Exception ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }

            finally
            {
                tcpClient.Close();
            }
            #endregion
            return ResponseMessage;
        }

        public string CreateResponseMessage(string status) =>
            "TCP " +
            " | " + DateTime.Now +
            " | " + _settings.Host.Normalize() +
            " | " + _settings.Port +
            " | " + status;
    }
}
