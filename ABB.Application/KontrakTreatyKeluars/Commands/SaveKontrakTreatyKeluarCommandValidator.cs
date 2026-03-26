using System.Collections.Generic;
using System.Linq;
using ABB.Application.KontrakTreatyKeluars.Queries;
using FluentValidation;

namespace ABB.Application.KontrakTreatyKeluars.Commands
{
    public class SaveKontrakTreatyKeluarCommandValidator : AbstractValidator<SaveKontrakTreatyKeluarCommand>
    {
        public SaveKontrakTreatyKeluarCommandValidator()
        {
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(HaveDetail).WithMessage("Detail Wajib diisi");
            
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(ShareMust100).WithMessage("Total share detail harus 100%");
        }
        
        private bool HaveDetail(List<DetailKontrakTreatyKeluarDataDto> details)
        {
            return details.Any();
        }
        
        private bool ShareMust100(List<DetailKontrakTreatyKeluarDataDto> details)
        {
            return details.Sum(s => s.pst_share) == 100;
        }
    }
}