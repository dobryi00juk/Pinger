using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Pinger.Interfaces;

namespace Pinger.Domain
{
    public class HttpPinger : IPinger
    {
        public event Action<string> ChangeStatus;
        private HttpStatusCode OldStatus { get; set; }
        private HttpStatusCode NewStatus { get; set; }
        private readonly ISettings _settings;
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly HttpClient _httpClient;

        public HttpPinger(HttpClient httpClient, ISettings settings, HttpRequestMessage httpRequestMessage)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestMessage = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));
            OldStatus = 0;
        }

        public async Task CheckStatusAsync()
        {
            
            var uri = new Uri("http://" + _settings.Host);

            _httpRequestMessage.Method = HttpMethod.Head;
            _httpRequestMessage.RequestUri = uri;

            try
            {
                var result = await _httpClient.SendAsync(_httpRequestMessage);
                var message = CreateResponseMessage(result.StatusCode);
                NewStatus = result.StatusCode;

                if (NewStatus != OldStatus)
                {
                    ChangeStatus?.Invoke(message);
                    OldStatus = NewStatus;
                }
            }
            catch (HttpRequestException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                ChangeStatus?.Invoke(ex.ToString());
            }
            finally
            {
                //сбрасываем значение sendStatusField для использования Http запросов в цикле
                ResetStatusField();
            }
        }

        private void ResetStatusField()
        {
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();//берем информацию о типо дял того что бы обнулить статус отправки
            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null)
                sendStatusField.SetValue(_httpRequestMessage, 0);
        }

        private string CreateResponseMessage(HttpStatusCode statusCode) =>
            "HTTP" +
            " | " + DateTime.Now +
            " | " + _httpRequestMessage.RequestUri.DnsSafeHost +
            " | " + statusCode.GetHashCode() +
            " | " + statusCode.ToString();

    }
}

//var message = /*"Http" + " | " + DateTime.Now + " | " + _httpRequestMessage.RequestUri.DnsSafeHost + " | " +
//         result.StatusCode.GetHashCode() + " | " + */result.StatusCode.ToString();