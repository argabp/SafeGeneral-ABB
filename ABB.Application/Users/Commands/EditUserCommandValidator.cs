using System.Text.RegularExpressions;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            var fileCondition = new FileValidatorCondition();

            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name maximum 50 characters");

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Last Name maximum 50 characters");

            RuleFor(p => p.RoleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role is required");

            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Email maximum 50 characters")
                .Must(ValidEmail).WithMessage("Invalid Email");

            RuleFor(p => p.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Phone Number maximum 50 characters");

            RuleFor(p => p.Address)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(255).WithMessage("Address maximum 255 characters");

            When(x => x.ProfilePhoto != null, () => {
                RuleFor(p => p.ProfilePhoto)
                    .Cascade(CascadeMode.Stop)
                    .Must(fileCondition.OnlyImageExtension).WithMessage("Incorrect File Type");
            });
        }
       
        private bool ValidEmail(string email)
        {
            if (email != null)
            {
                bool result = Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                return result;
            }
            return true;
        }
    }
}