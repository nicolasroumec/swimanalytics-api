using FluentValidation;
using swimanalytics.Models.DTOs;
using System.Text.RegularExpressions;

namespace swimanalytics.Tools.Validators
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is not valid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(new Regex(@"[A-Z]+")).WithMessage("Password must contain at least one uppercase letter")
                .Matches(new Regex(@"[a-z]+")).WithMessage("Password must contain at least one lowercase letter")
                .Matches(new Regex(@"[0-9]+")).WithMessage("Password must contain at least one number")
                .Matches(new Regex(@"[\W_]+")).WithMessage("Password must contain at least one special character");
        }
    }
}
