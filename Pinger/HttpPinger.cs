using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;

namespace Pinger
{
    internal class HttpPinger : IPinger
    {
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly Logger _logger;
        private readonly HttpClient _httpClient;
        private readonly UriBuilder _uriBuilder;

        public HttpPinger(Logger logger, HttpRequestMessage httpRequestMessage, HttpClient httpClient, UriBuilder uriBuilder)
        {
            _httpRequestMessage = httpRequestMessage;
            _logger = logger;
            _httpClient = httpClient;
            _uriBuilder = uriBuilder;
            _logger = logger;
        }

        public void CheckStatus()
        {
            _httpRequestMessage.RequestUri = _uriBuilder.Uri;
            _httpRequestMessage.Method = HttpMethod.Head;
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();

            try
            {
                var result = _httpClient.SendAsync(_httpRequestMessage).Result.StatusCode;
                var message = DateTime.Now + " | " + _httpRequestMessage.RequestUri.DnsSafeHost + " | " +
                              result.GetHashCode() + " | " + result;
                _logger.LogToFileAndConsole(message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogToFileAndConsole(DateTime.Now + "|" + this.GetType() + "|" + ex.Message);
            }
            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null) sendStatusField.SetValue(_httpRequestMessage, 0);
        }
    }
}

