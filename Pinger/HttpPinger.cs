using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Reflection;

namespace Pinger
{
    class HttpPinger 
    {
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly HttpClient _httpClient;
        private readonly UriBuilder _uriBuilder;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public HttpPinger(HttpRequestMessage httpRequestMessage, HttpClient httpClient, UriBuilder uriBuilder, ILogger<HttpPinger> logger, IConfiguration configuration)
        {
            _httpRequestMessage = httpRequestMessage;
            _httpClient = httpClient;
            _uriBuilder = uriBuilder;
            _logger = logger;
            _configuration = configuration;
        }

        public string CheckStatus()
        {
            _httpRequestMessage.RequestUri = _uriBuilder.Uri;
            _httpRequestMessage.Method = HttpMethod.Head;
            var message = String.Empty;

            TypeInfo requestType = _httpRequestMessage.GetType().GetTypeInfo();

            try
            {
                var result = _httpClient.SendAsync(_httpRequestMessage).Result.StatusCode;
                message = DateTime.Now + " | " + _httpRequestMessage.RequestUri.DnsSafeHost + " | " + result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogInformation(ex.Message);
            }

            FieldInfo sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null) sendStatusField.SetValue(_httpRequestMessage, 0);

            return message;
        }
    }
}

