using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PingerLib.Configuration;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class App 
    {
        private readonly IPingerFactory _pingerFactory;
        private readonly ILogger _logger;

        public App(IPingerFactory pingerFactory, ILogger logger)
        {
            _pingerFactory = pingerFactory ?? throw new ArgumentNullException(nameof(pingerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Start(IEnumerable<IHost> hosts, CancellationTokenSource cts)
        {
            if (hosts == null) throw new ArgumentNullException(nameof(hosts));
            if (cts == null) throw new ArgumentNullException(nameof(cts));

            foreach (var host in hosts)
            {
                if (cts.Token.IsCancellationRequested)
                    return;
            
                ThreadPool.QueueUserWorkItem(CallBack, host);
            }
        }

        private void CallBack(object state)
        {
            var host = state as Host;
            Run(host, new CancellationToken()).Wait();
        }

        private async Task Run(IHost host, CancellationToken token)
        {
            IPinger pinger = null;

            try
            {
                pinger = _pingerFactory.CreatePinger(host);

                while (true)
                {
                    var result = await pinger.GetStatusAsync(token);
                    
                    if(result.StatusChanged)
                        _logger.Log(CreateResponseMessage(result));
                }
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == token)
            {
                if (pinger != null) _logger.Log(pinger.GetType() + " | " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }
        }

        private static string CreateResponseMessage(PingResult result)
        {
            return result.StatusCode == null
                ? (_ = $"{result.Protocol} | {result.Date} | {result.Host} | {result.Status}")
                : $"{result.Protocol} | {result.Date} | {result.Host} | {result.Status} | Status code: {result.StatusCode}";
        }
    }
}

