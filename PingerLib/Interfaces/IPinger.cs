using System;
using System.Threading.Tasks;

namespace PingerLib.Interfaces
{
    public interface IPinger
    {
        Task<string> CheckStatusAsync();
        public event Action<string> ChangeStatus;
    }
}
