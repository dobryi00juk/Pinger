using System.Text.RegularExpressions;
using FluentValidation;

namespace PingerLib.Configuration.Rules
{
    public class HostSettingsRules : AbstractValidator<Host>
    {
        public HostSettingsRules()
        {
            RuleFor(x => x.HostName)
                .NotEmpty()
                .Must(uri => Regex.IsMatch(uri, @"^(www\.)?\w+\.(ru|org|net|com)$"))
                .WithMessage("host address unknown!");

            RuleFor(x => x.Period)
                .Must(p => p >= 0)
                .WithMessage("Period must be greater then 0.");

            RuleFor(x => x.Protocol)
                .NotEmpty();
        }
    }
}
