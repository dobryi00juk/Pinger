using System.Threading.Tasks;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        string ResponseMessage { get; set; }
        Task<string> CheckStatusAsync();
        string CreateResponseMessage(string status);
    }
}
