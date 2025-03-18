using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ABB.Application.UserCabangs.Commands
{
    public class AddUserCabangCommandValidator : AbstractValidator<AddUserCabangCommand>
    {
        public AddUserCabangCommandValidator()
        {
            RuleFor(p => p.userid)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("User wajib dipilih");
            
            RuleFor(p => p.Cabangs)
                .Cascade(CascadeMode.Stop)
                .Must(UniqueCabang).WithMessage("Cabang dan Database wajib diisi.");
        }
        
        private bool UniqueCabang(List<string> cabangs)
        {
            return cabangs.GroupBy(cabang => cabang).All(cabang => cabang.Count() == 1);
        }
    }
}