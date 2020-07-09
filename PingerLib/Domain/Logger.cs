using Pinger.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pinger
{
    public class Logger : ILogger
    {
        public void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }

        public void LogToFile(string message)
        {
            using var writer = File.AppendText("log.txt");
            writer.WriteLine($"{message}");
        }

        public void LogToFileAndConsole(string message)
        {
            Console.WriteLine(message);
            using var writer = File.AppendText("log.txt");
            writer.WriteLine($"{message}");
        }
    }
}
