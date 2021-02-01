using PingerLib.Interfaces;
using System;
using System.IO;

namespace PingerLib.Domain
{
    public class Logger : ILogger
    {
        private readonly object _locker = new object();

        public void Log(string message)
        {
            lock(_locker)
            {
                using var write = File.AppendText("log.txt");
                write.WriteLine($"{message}");
                Console.WriteLine(message);
            }
        }
    }
}
