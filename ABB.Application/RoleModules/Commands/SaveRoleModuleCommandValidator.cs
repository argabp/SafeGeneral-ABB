using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ABB.Application.RoleModules.Commands
{
    public class SaveRoleModuleCommandValidator : AbstractValidator<SaveRoleModuleCommand>
    {
        public SaveRoleModuleCommandValidator()
        {
            RuleFor(p => p.RoleId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Role wajib diisi");
            RuleFor(p => p.Modules)
                .Cascade(CascadeMode.Stop)
                .Must(UniqueModule).WithMessage("Module tidak boleh di input lebih dari sekali.");
        }
        
        private bool UniqueModule(List<int> modules)
        {
            return modules.GroupBy(module => module).All(module => module.Count() == 1);
        }
    }
}