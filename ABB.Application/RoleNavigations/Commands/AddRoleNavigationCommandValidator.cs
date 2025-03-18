using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ABB.Application.RoleNavigations.Commands
{
    public class AddRoleNavigationCommandValidator : AbstractValidator<AddRoleNavigationCommand>
    {
        public AddRoleNavigationCommandValidator()
        {
            RuleFor(p => p.RoleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role wajib dipilih");
            
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