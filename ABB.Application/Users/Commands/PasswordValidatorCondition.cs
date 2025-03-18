using System.Linq;
using System.Text.RegularExpressions;
using ABB.Application.Common.Helpers;
using ABB.Application.Common.Interfaces;
using FluentValidation;

namespace ABB.Application.Users.Commands
{
    public class PasswordValidatorCondition
    {
        private readonly IDbContext _context;

        public PasswordValidatorCondition(IDbContext context)
        {
            _context = context;
        }

        public void EqualConfirmPassword<T>(string confirmpassword, ValidationContext<T> context)
        {
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            string newpassword = DynamicHelper.GetValue(model, propertyName.Replace(" ", ""));
            if (confirmpassword != newpassword)
                context.AddFailure($"{propertyName} and Confirm Password do not match");
        }

        public void AtLeast8Character<T>(string password, ValidationContext<T> context)
        {
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            if (password?.Length < 8)
                context.AddFailure($"{propertyName} must be at least 8 characters long");
        }

        public void NotContainRepeatedLetter<T>(string password, ValidationContext<T> context)
        {
            var sequenceLength = 3;
            var pattern = "(.)\\1{" + (sequenceLength - 1) + "}";
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            if (Regex.IsMatch(password?.ToLower(), pattern))
                context.AddFailure($"{propertyName} can not contain repeated letters more than twice");
        }

        public void NotContainSpace<T>(string password, ValidationContext<T> context)
        {
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            if (password?.Contains(" ") ?? false)
                context.AddFailure($"{propertyName} can not contain space character");
        }

        public void HasSpecialCharacter<T>(string password, ValidationContext<T> context)
        {
            string pattern = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])";
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            if (!Regex.IsMatch(password, pattern) || !IsMatchSpecialCharacter(password))
                context.AddFailure(propertyName +
                                   " must contain uppercase, lowercase, numeric, and one of these characters(`~!@#$%^&*()_+=-{}[]|\\;':\",.<>/?)");
        }

        private bool IsMatchSpecialCharacter(string password)
        {
            bool hasSpChar = false;
            string spChar = "`~!@#$%^&*()_+=-{}[]|\\;':\",.<>/?";
            for (int i = 0; i < password.Length; i++)
            {
                if (spChar.IndexOf(password[i]) >= 0)
                {
                    hasSpChar = true;
                    break;
                }
            }

            return hasSpChar;
        }

        public void NotContainFirstAndLastName<T>(string password, ValidationContext<T> context)
        {
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            string id = model.Id;
            var user = _context.User.FirstOrDefault(a => a.Id == id);
            string firstname, lastname;
            if (user == null)
            {
                firstname = model.FirstName?.ToLower() ?? string.Empty;
                lastname = model.LastName?.ToLower() ?? string.Empty;
            }
            else
            {
                firstname = user?.FirstName?.ToLower() ?? string.Empty;
                lastname = user?.LastName?.ToLower() ?? string.Empty;
            }

            if (firstname == "" && lastname == "") return;
            var containFirstName = password.ToLower().Contains(firstname);
            var containLastName = password.ToLower().Contains(lastname) && !string.IsNullOrEmpty(lastname);
            if (containFirstName || containLastName)
                context.AddFailure($"{propertyName} can not contain First Name and Last Name");
        }

        public void NotContainLastSixPassword<T>(string password, ValidationContext<T> context)
        {
            var model = context.InstanceToValidate as dynamic;
            var propertyName = DynamicHelper.PropertyExist(model, "Password") ? "Password" : "New Password";
            string id = model.Id;
            var OldPasswordList = (from u in _context.UserHistory
                    where u.UserId == id && u.Password != null
                    select new { u.Id, u.Password }
                ).Distinct().OrderByDescending(u => u.Id).Take(6).ToList();

            var IsEqualWithLastPass = false;

            if (OldPasswordList.Count > 0)
            {
                foreach (var pass in OldPasswordList)
                {
                    if (Encryption.Verify(password, pass.Password))
                        IsEqualWithLastPass = true;
                }
            }

            if (IsEqualWithLastPass)
                context.AddFailure($"{propertyName} can't contain last 6 previous password");
        }
    }
}