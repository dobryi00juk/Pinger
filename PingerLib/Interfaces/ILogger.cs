using System;
using System.Collections.Generic;
using System.Text;

namespace Pinger.Interfaces
{
    interface ILogger
    {
        void LogToConsole(string message);
        void LogToFile(string message);
        void LogToFileAndConsole(string message);
    }
}
