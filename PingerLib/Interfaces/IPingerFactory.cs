namespace PingerLib.Interfaces
{
    public interface IPingerFactory
    {
        IPinger CreatePinger(IHost host);
    }
}
