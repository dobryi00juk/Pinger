﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Pinger.Interfaces;
using PingerLib.Interfaces;

namespace Pinger.Domain
{
    public class HttpPinger : IPinger
    {
        public event Action<string> ChangeStatus;
        private HttpStatusCode OldStatus { get; set; }
        private HttpStatusCode NewStatus { get; set; }
        private int StatusCode { get; set; }
        public string ResponseMessage { get; set; }
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

        public async Task<string> CheckStatusAsync()
        {
            //Uri.TryCreate("http://" + _settings.Host, UriKind.Absolute, out var uri);
            //if (IsValidUri("http://" + _settings.Host))
            //{
            //    var uri = new Uri("http://" + _settings.Host);
            //    _httpRequestMessage.Method = HttpMethod.Head;
            //    _httpRequestMessage.RequestUri = uri;
            //}
            var uri = new Uri("http://" + _settings.Host);
            _httpRequestMessage.Method = HttpMethod.Head;
            _httpRequestMessage.RequestUri = uri;

            try
            {
                var result = await _httpClient.SendAsync(_httpRequestMessage);
                NewStatus = result.StatusCode;
                StatusCode = (int)NewStatus;
                ResponseMessage = CreateResponseMessage(NewStatus.ToString());

                if (NewStatus != OldStatus)
                {
                    ChangeStatus?.Invoke(ResponseMessage);
                    OldStatus = NewStatus;
                }
            }

            #region catch

            catch (HttpRequestException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (InvalidOperationException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            catch (UriFormatException ex)
            {
                ResponseMessage = CreateResponseMessage(ex.Message);
                ChangeStatus?.Invoke(ResponseMessage);
            }
            finally
            {
                //сбрасываем значение sendStatusField для использования Http запросов в цикле
                ResetStatusField();
            }

            #endregion

            return ResponseMessage;
        }

        private void ResetStatusField()
        {
            var requestType = _httpRequestMessage.GetType().GetTypeInfo();//берем информацию о типо для того что бы обнулить статус отправки
            var sendStatusField = requestType.GetField("_sendStatus", BindingFlags.Instance | BindingFlags.NonPublic);

            if (sendStatusField != null)
                sendStatusField.SetValue(_httpRequestMessage, 0);
        }

        public string CreateResponseMessage(string status) =>
            "HTTP" +
            " | " + DateTime.Now +
            " | " + _httpRequestMessage.RequestUri +
            " | " + StatusCode +
            " | " + status;

        //private bool IsValidUri(string uri)
        //{
        //    return Uri.IsWellFormedUriString(uri, UriKind.Absolute);
        //}
    }

}

  