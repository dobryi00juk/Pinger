namespace PingerLib.Interfaces
{
    public interface ILogger
    {
        void LogToConsole(string message);
        void LogToFile(string message);
        void LogToFileAndConsole(string message);
    }
}
