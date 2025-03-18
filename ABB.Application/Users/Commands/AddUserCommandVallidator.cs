using System.Linq;
using System.Text.RegularExpressions;
using ABB.Application.Common.Commands;
using ABB.Application.Common.Interfaces;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        private readonly IDbContext _context;

        public AddUserCommandValidator(IDbContext context)
        {
            _context = context;
            var inputValidator = new InputValidatorCondition();
            var condition = new PasswordValidatorCondition(context);
            var fileCondition = new FileValidatorCondition();

            RuleFor(p => p.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username is required")
                .Must(UserExist).WithMessage("Username is already used")
                .MaximumLength(50).WithMessage("Username maximum 50 characters")
                .Matches(@"\A\S+\z").WithMessage("Username contains space");

            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name maximum 50 characters")
                .Custom(inputValidator.AlphabetOnly);

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Last Name maximum 50 characters")
                .Custom(inputValidator.AlphabetOnly);

            RuleFor(p => p.RoleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role is required");

            RuleFor(p => p.Email)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Email maximum 50 characters")
                .Must(ValidEmail).WithMessage("Invalid Email");

            RuleFor(p => p.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(50).WithMessage("Phone Number maximum 50 characters")
                .Custom(inputValidator.NumericOnly);

            RuleFor(p => p.Address)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(255).WithMessage("Address maximum 255 characters")
                .Custom(inputValidator.AlphaNumericWithCommonCharacter);

            RuleFor(p => p.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .Custom(condition.AtLeast8Character)
                .Custom(condition.HasSpecialCharacter)
                .Custom(condition.NotContainLastSixPassword)
                // .Custom(condition.NotContainFirstAndLastName)
                .Custom(condition.NotContainRepeatedLetter)
                .Custom(condition.NotContainSpace)
                .MaximumLength(50).WithMessage("Password maximum 50 characters");

            RuleFor(p => p.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Custom(condition.EqualConfirmPassword);

            When(x => x.ProfilePhoto != null, () =>
            {
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

        private bool UserExist(string username)
        {
            return !_context.User.Any(a => a.UserName == username);
        }
    }
}