using FluentValidation;
using System.Text.RegularExpressions;

namespace PingerLib.Configuration
{
    public class SettingsRules : AbstractValidator<Host>
    {
        public SettingsRules()
        {
            RuleFor(x => x.HostName)
                .NotEmpty()
                .Must(uri => Regex.IsMatch(uri, @"^(www\.)?\w+\.(ru|org|net|com)$"))
                .WithMessage("Адрес хоста не известен!");


            //RuleFor(x => x.Port)
            //    .InclusiveBetween(1, 65536);
            //    //.NotEmpty();

            RuleFor(x => x.Period)
                .Must(p => p >= 0)
                .WithMessage("Period должен быть больше либо равно 0.");

            RuleFor(x => x.Protocol)
                .NotEmpty();
        }
    }
}
