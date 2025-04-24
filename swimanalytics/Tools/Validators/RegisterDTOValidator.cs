using FluentValidation;
using swimanalytics.Models.DTOs;
using System.Text.RegularExpressions;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Selected gender is not valid");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0")
            .LessThan(300).WithMessage("Height must be less than 300 cm");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0")
            .LessThan(500).WithMessage("Weight must be less than 500 kg");

        RuleFor(x => x.Wingspan)
            .GreaterThan(0).WithMessage("Wingspan must be greater than 0")
            .LessThan(300).WithMessage("Wingspan must be less than 300 cm");

        RuleFor(x => x.Club)
            .MaximumLength(100).WithMessage("Club name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not valid");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(new Regex(@"^\+?[0-9]{8,15}$")).WithMessage("Phone number format is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(new Regex(@"[A-Z]+")).WithMessage("Password must contain at least one uppercase letter")
            .Matches(new Regex(@"[a-z]+")).WithMessage("Password must contain at least one lowercase letter")
            .Matches(new Regex(@"[0-9]+")).WithMessage("Password must contain at least one number")
            .Matches(new Regex(@"[\W_]+")).WithMessage("Password must contain at least one special character");

        RuleFor(x => x.role)
            .IsInEnum().WithMessage("Selected role is not valid");
    }
}