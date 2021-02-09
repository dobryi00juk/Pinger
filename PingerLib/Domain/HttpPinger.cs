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
        private readonly IHost _host;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private bool _statusChanged;
        private int _oldStatus;
        private int _newStatus;

        public HttpPinger(HttpClient httpClient, HttpRequestMessage httpRequestMessage, IHost host, ILogger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logger = logger ?? throw  new ArgumentNullException(nameof(logger));
        }

        public async Task<PingResult> GetStatusAsync(CancellationToken token)
        {
            var status = await CheckStatusAsync(_host.HostName, token);

            if (token.IsCancellationRequested)
                _logger.Log("HttpPinger:GetStatusAsync method canceled");

            return new PingResult
            {
                Protocol = "http",
                Date = DateTime.Now,
                Host = _host.HostName,
                Status = (int) status == _host.StatusCode 
                    ? status.ToString() 
                    : $"Error! (Expected status code: {_host.StatusCode})",
                StatusCode = (int)status,
                StatusChanged = _statusChanged
            };
        }

        private async Task<HttpStatusCode> CheckStatusAsync(string host, CancellationToken token)
        {
            try
            {
                _httpRequestMessage.Method = HttpMethod.Head;
                _httpRequestMessage.RequestUri = new Uri("http://" + host);

                await Task.Delay(_host.Period * 1000, token);
                
                if (token.IsCancellationRequested)
                {
                    _logger.Log("HttpPinger:CheckStatusAsync method canceled!");
                    throw new OperationCanceledException(token);
                }

                var result = await _httpClient.SendAsync(_httpRequestMessage, token);
                _newStatus = (int) result.StatusCode;

                if (_newStatus != _oldStatus)
                {
                    _statusChanged = true;
                    _oldStatus = _newStatus;
                }
                else
                    _statusChanged = false;

                return result.StatusCode;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                throw;
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

