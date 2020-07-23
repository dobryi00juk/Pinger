using System.Threading.Tasks;

namespace Pinger.Interfaces
{
    public interface IPinger
    {
        Task<string> CheckStatusAsync();
    }
}
