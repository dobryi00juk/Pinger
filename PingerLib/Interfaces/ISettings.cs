using FluentValidation.Results;

namespace PingerLib.Interfaces
{
    public interface ISettings
    {
        public string Host { get; }
        public int Period { get; }
        public int Port { get; }
        public ValidationResult ValidationResult { get; set; }
    }
}
