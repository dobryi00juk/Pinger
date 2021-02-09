using FluentValidation.Results;
using System.Collections.Generic;

namespace PingerLib.Interfaces
{
    public interface ISettings
    {
        IEnumerable<IHost> HostList { get; set; }
        ValidationResult ValidationResult { get; set; }
    }
}
