using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        Task GetStatusAsync(string host, int period, CancellationToken cts);
        event Action<string> ErrorOccured;
    }
}
