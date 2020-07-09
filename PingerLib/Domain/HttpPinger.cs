using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Pinger.Interfaces;
using Pinger.Configuration;

namespace Pinger.Domain
{
    internal class HttpPinger : IPinger
    {
        private readonly ISettings _settings;
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        //private readonly UriBuilder _uriBuilder;

        public HttpPinger(ILogger logger, HttpClient httpClient, ISettings settings, HttpRequestMessage httpRequestMessage)
        {
            _settings = settings;
            _httpClient = httpClient;
            _httpRequestMessage = httpRequestMessage;
            _logger = logger;
        }

        public string CheckStatus()
        {
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();
            _httpRequestMessage.Method = HttpMethod.Head;
            var uri = new Uri("http://" + _settings.Host);
            _httpRequestMessage.RequestUri = uri;

            var result = _httpClient.SendAsync(_httpRequestMessage).Result.StatusCode;
                var message = DateTime.Now + " | " + _httpRequestMessage.RequestUri.DnsSafeHost + " | " +
                              result.GetHashCode() + " | " + result;

            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null)
                sendStatusField.SetValue(_httpRequestMessage, 0);

            return message;
        }
    }
}

