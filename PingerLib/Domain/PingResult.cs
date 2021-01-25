using System;
using System.Collections.Generic;
using System.Text;

namespace PingerLib.Domain
{
    public class PingResult
    {
        public string Protocol { get; set; }
        public DateTime Date { get; set; }
        public string Host { get; set; }
        public string Status { get; set; }
        public int? StatusCode { get; set; }
        public bool StatusChanged { get; set; }
    }
}
