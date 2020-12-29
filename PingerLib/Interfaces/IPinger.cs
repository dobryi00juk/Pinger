using System;
using System.Threading.Tasks;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        Task GetStatusAsync(string host, int period);
        event Action<string> ErrorOccured;
    }
}
