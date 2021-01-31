using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace PingerLib.Interfaces.Wrappers
{
    public interface IPingWrapper
    {
        Task<IPStatus> SendPingAsync(string hostNameOrAddress, int timeout);
    }
}