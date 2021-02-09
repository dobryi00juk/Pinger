using PingerLib.Interfaces;

namespace PingerLib.Configuration
{
    public class Host : IHost
    {
        public string HostName { get; set; }
        public string Protocol { get; set; }
        public int Period { get; set; }
        public int? StatusCode { get; set; }
    }
}
