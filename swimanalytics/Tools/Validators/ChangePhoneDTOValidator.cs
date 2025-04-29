using FluentValidation;
using swimanalytics.Models.DTOs;
using System.Text.RegularExpressions;

namespace swimanalytics.Tools.Validators
{
    public class ChangePhoneDTOValidator : AbstractValidator<ChangePhoneDTO>
    {
        public ChangePhoneDTOValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(new Regex(@"^\+?[0-9]{8,15}$")).WithMessage("Phone number format is not valid");
        }
    }
}
