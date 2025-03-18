using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class ChangeCurrentPasswordCommandValidator : AbstractValidator<ChangeCurrentPasswordCommand>
    {
        private readonly IUserManagerHelper _userManager;

        public ChangeCurrentPasswordCommandValidator(IDbContext context, IUserManagerHelper userManager)
        {
            _userManager = userManager;
            var condition = new PasswordValidatorCondition(context);
            RuleFor(p => p.CurrentPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .Must(MatchCurrentPassword).WithMessage("Invalid Password");
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

        private bool MatchCurrentPassword(ChangeCurrentPasswordCommand model, string password)
        {
            var user = _userManager.FindByIdAsync(model.Id).Result;
            var validPassword = _userManager.CheckPasswordAsync(user, password).Result;
            return validPassword;
        }
    }
}