using System.Threading.Tasks;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        Task<string> CheckStatusAsync();
    }
}
