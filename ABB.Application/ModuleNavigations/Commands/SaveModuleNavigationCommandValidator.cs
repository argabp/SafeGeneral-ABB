using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ABB.Application.ModuleNavigations.Commands
{
    public class SaveModuleNavigationCommandValidator : AbstractValidator<SaveModuleNavigationCommand>
    {
        public SaveModuleNavigationCommandValidator()
        {
            RuleFor(p => p.ModuleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Module wajib dipilih");
            
            RuleFor(p => p.Navigations)
                .Cascade(CascadeMode.Stop)
                .Must(UniqueNavigation).WithMessage("Menu tidak boleh di input lebih dari sekali.");
        }
        
        private bool UniqueNavigation(List<int> navigations)
        {
            return navigations.GroupBy(navigation => navigation).All(navigation => navigation.Count() == 1);
        }
    }
}