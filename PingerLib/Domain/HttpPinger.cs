using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class HttpPinger : IPinger
    {
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        public event Action<string> ErrorOccured;
        private bool _statusChanged;
        private int _oldStatus;
        private int _newStatus;

        public HttpPinger(HttpClient httpClient, HttpRequestMessage httpRequestMessage, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
            _logger = logger ?? throw  new ArgumentNullException(nameof(logger));
        }

        public async Task GetStatusAsync(string host, int period)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            while (true)
            {
                var status = await CheckStatusAsync(host);

                if (_statusChanged)
                    _logger.LogToConsole($"Http | {DateTime.Now} | {host} | {status} | {(int)status}");
                
                Thread.Sleep(period * 1000);
            }
        }

        private async Task<HttpStatusCode> CheckStatusAsync(string host)
        {
            _httpRequestMessage.Method = HttpMethod.Head;
            _httpRequestMessage.RequestUri = new Uri("http://" + host);

            try
            {
                var result = await _httpClient.SendAsync(_httpRequestMessage);
                _newStatus = (int) result.StatusCode;

                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                {
                    _statusChanged = false;
                }

                return result.StatusCode;
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(ex.Message);
                return HttpStatusCode.BadRequest;
            }
            finally
            {
                //сбрасываем значение sendStatusField для использования Http запросов в цикле
                ResetStatusField();
            }
        }

        private void ResetStatusField()
        {
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();//берем информацию о типе для того что бы обнулить статус отправки
            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null)
                sendStatusField.SetValue(_httpRequestMessage, 0);
        }
    }
}

