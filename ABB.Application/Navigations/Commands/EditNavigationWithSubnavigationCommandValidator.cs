using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ABB.Application.Navigations.Commands
{
    public class EditNavigationWithSubnavigationCommandValidator : AbstractValidator<EditNavigationWithSubnavigationCommand>
    {
        public EditNavigationWithSubnavigationCommandValidator()
        {
            RuleFor(p => p.SubNavigationId)
                .Cascade(CascadeMode.Stop)
                .Must(UniqueNavigation).WithMessage("Menu tidak boleh di input lebih dari sekali.");
        }
        
        private bool UniqueNavigation(List<int> navigations)
        {
            return navigations.GroupBy(navigation => navigation).All(navigation => navigation.Count() == 1);
        }
    }
}