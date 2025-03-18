using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class ChangeUserProfileCommandValidator : AbstractValidator<ChangeUserProfileCommand>
    {
        public ChangeUserProfileCommandValidator()
        {
            var condition = new FileValidatorCondition();

            When(x => x.ProfilePhoto != null, () =>
            {
                RuleFor(p => p.ProfilePhoto)
                    .Cascade(CascadeMode.Stop)
                    .Must(condition.OnlyImageExtension).WithMessage("Incorrect File Type");
            });

            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name maximum 50 characters");

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Last Name maximum 50 characters");

            RuleFor(p => p.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone Number is required")
                .MaximumLength(50).WithMessage("Phone Number maximum 50 characters");

            RuleFor(p => p.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(255).WithMessage("Address maximum 255 characters");
        }
    }
}