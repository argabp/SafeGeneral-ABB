using System.Text.RegularExpressions;
using FluentValidation;

namespace ABB.Application.Common.Commands
{
    public class InputValidatorCondition
    {
        public void AlphaNumericWithCommonCharacter<T>(string input, ValidationContext<T> context)
        {
            if (string.IsNullOrEmpty(input)) return;
            string pattern = @"^[\w .,'/]+$";
            if (!Regex.IsMatch(input, pattern))
                context.AddFailure("Only alphabet and numeric are allowed");
        }

        public void AlphaNumericOnly<T>(string input, ValidationContext<T> context)
        {
            if (string.IsNullOrEmpty(input)) return;
            string pattern = @"^[\w]+$";
            if (!Regex.IsMatch(input, pattern))
                context.AddFailure("Only alphabet and numeric are allowed");
        }

        public void AlphaNumericOnlyWithSpace<T>(string input, ValidationContext<T> context)
        {
            if (string.IsNullOrEmpty(input)) return;
            string pattern = @"^[\w ]+$";
            if (!Regex.IsMatch(input, pattern))
                context.AddFailure("Only alphabet and numeric are allowed");
        }

        public void AlphabetOnly<T>(string input, ValidationContext<T> context)
        {
            if (string.IsNullOrEmpty(input)) return;
            string pattern = "^[a-zA-Z ]+$";
            if (!Regex.IsMatch(input, pattern))
                context.AddFailure("Only alphabet is allowed");
        }

        public void NumericOnly<T>(string input, ValidationContext<T> context)
        {
            if (string.IsNullOrEmpty(input)) return;
            string pattern = @"^\d+$";
            if (!Regex.IsMatch(input, pattern))
                context.AddFailure("Only numeric is allowed");
        }
    }
}