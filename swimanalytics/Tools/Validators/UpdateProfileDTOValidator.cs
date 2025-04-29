using FluentValidation;
using swimanalytics.Models.DTOs;

namespace swimanalytics.Tools.Validators
{
    public class UpdateProfileDTOValidator : AbstractValidator<UpdateProfileDTO>
    {
        public UpdateProfileDTOValidator()
        {
            When(x => x.FirstName != null, () => {
                RuleFor(x => x.FirstName)
                    .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");
            });

            When(x => x.LastName != null, () => {
                RuleFor(x => x.LastName)
                    .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");
            });

            // Gender es un tipo por valor, así que necesitaríamos otra estrategia si quisieramos hacerlo opcional
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

            When(x => x.Club != null, () => {
                RuleFor(x => x.Club)
                    .MaximumLength(100).WithMessage("Club name cannot exceed 100 characters");
            });
        }
    }
}
