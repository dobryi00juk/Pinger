namespace PingerLib.Interfaces
{
    public interface IHost
    {
        string HostName { get; set; }
        string Protocol { get; set; }
        int Period { get; set; }
        int? StatusCode { get; set; }
    }
}
