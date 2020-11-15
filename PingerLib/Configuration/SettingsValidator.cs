using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PingerLib.Configuration
{
    public class SettingsValidator : AbstractValidator<Settings>
    {
        public SettingsValidator()
        {
            RuleFor(x => x.Host)
                .NotEmpty()
                .Must(uri => Regex.IsMatch(uri, @"^(http | http(s) ?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?"))
                .WithMessage("Wrong Url");

            RuleFor(x => x.Port).InclusiveBetween(1, 65536)
                .NotEmpty();

            RuleFor(x => x.Period)
                .NotEmpty()
                .Must(p => p > 0)
                .WithMessage("Period > 0");
        }
    }
}
