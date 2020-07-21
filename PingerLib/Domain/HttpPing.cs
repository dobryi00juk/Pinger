using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class HttpPing : IPinger
    {
        public event Action<string> ChangeStatus;
        private HttpStatusCode OldStatus { get; set; }
        private HttpStatusCode NewStatus { get; set; }
        private readonly ISettings _settings;
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly HttpClient _httpClient;

        public HttpPing(HttpClient httpClient, ISettings settings, HttpRequestMessage httpRequestMessage)
        {
            _settings = settings;
            _httpClient = httpClient;
            _httpRequestMessage = httpRequestMessage;
            OldStatus = 0;
        }

        public async Task<string> CheckStatusAsync()
        {
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();//берем информацию о типо дял того что бы обнулить статус отправки
            var uri = new Uri("http://" + _settings.Host);//имя хоста из файла настроек

            //настраиваем HttpRequestMessage
            _httpRequestMessage.Method = HttpMethod.Head;
            _httpRequestMessage.RequestUri = uri;

            var result = await _httpClient.SendAsync(_httpRequestMessage);//.Result.StatusCode;
            var message = "Http" + " | " + DateTime.Now + " | " + _httpRequestMessage.RequestUri.DnsSafeHost + " | " +
                     result.StatusCode.GetHashCode() + " | " + result.StatusCode;
            NewStatus = result.StatusCode;

            if (NewStatus != OldStatus)
            {
                ChangeStatus?.Invoke(message);
                OldStatus = NewStatus;
            }

            //сбрасываем значение sendStatusField для использования Http запросов в цикле
            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null)
                sendStatusField.SetValue(_httpRequestMessage, 0);

            return message;
        }
    }
}

