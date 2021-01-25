using System;
using System.IO;
using PingerLib.Interfaces;

namespace PingerLib.Domain
{
    public class Logger : ILogger
    {
        private readonly object _locker;
        public Logger()
        {
            _locker = new object();
        }
        
        public void Log(string message)
        {
            Console.WriteLine(message);

            lock (_locker)
            {
                using var writer = File.AppendText("log.txt");
                writer.WriteLine($"{message}");
            }
        }
    }
}
