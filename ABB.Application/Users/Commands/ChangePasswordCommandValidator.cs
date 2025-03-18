using ABB.Application.Common.Interfaces;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator(IDbContext context)
        {
            var condition = new PasswordValidatorCondition(context);
            RuleFor(p => p.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("New Password is required")
                .Custom(condition.AtLeast8Character)
                .Custom(condition.HasSpecialCharacter)
                .Custom(condition.NotContainLastSixPassword)
                .Custom(condition.NotContainFirstAndLastName)
                .Custom(condition.NotContainRepeatedLetter)
                .Custom(condition.NotContainSpace)
                .MaximumLength(50).WithMessage("New Password maximum 50 characters");

            RuleFor(p => p.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Confirm Password is required")
                .Custom(condition.EqualConfirmPassword);
        }
    }
}