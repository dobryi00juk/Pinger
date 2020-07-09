using System;
using System.Collections.Generic;
using System.Text;

namespace Pinger.Interfaces
{
    interface ISettings
    {
        public string Host { get; }
        public int Period { get; }
        public int Port { get; }
        public string Protocol { get; }
    }
}
