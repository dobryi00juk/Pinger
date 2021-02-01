using System.Threading;
using System.Threading.Tasks;
using PingerLib.Domain;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        Task<PingResult> GetStatusAsync(CancellationToken cts);
    }
}
