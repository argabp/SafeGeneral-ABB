using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationException = ABB.Application.Common.Exceptions.ValidationException;

namespace ABB.Web.Extensions
{
    public static class ModelStateErrorExtension
    {
        public static void AddModelErrors(this ModelStateDictionary modelstate, ValidationException ex,
            string propertyName = "")
        {
            var PrefixProperty = propertyName == "" ? string.Empty : $"{propertyName}.";
            foreach (var err in ex.Errors)
            {
                foreach (var message in err.Value)
                {
                    modelstate.AddModelError($"{PrefixProperty}{err.Key}", message);
                }
            }
        }

        public static bool IsValid<T>(this ModelStateDictionary modelstate, AbstractValidator<T> validator, T model,
            string propertyName = "")
        {
            var validationResult = validator.Validate(model);
            var PrefixProperty = propertyName == "" ? string.Empty : $"{propertyName}.";
            var result = validationResult.IsValid;
            if (!result)
            {
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelstate.AddModelError($"{PrefixProperty}{failure.PropertyName}", failure.ErrorMessage);
                }
            }

            return result;
        }
    }
}