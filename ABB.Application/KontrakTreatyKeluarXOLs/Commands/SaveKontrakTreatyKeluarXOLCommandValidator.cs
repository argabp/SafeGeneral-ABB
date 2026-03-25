using System.Collections.Generic;
using System.Linq;
using ABB.Application.KontrakTreatyKeluarXOLs.Queries;
using FluentValidation;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Commands
{
    public class SaveKontrakTreatyKeluarXOLCommandValidator : AbstractValidator<SaveKontrakTreatyKeluarXOLCommand>
    {
        public SaveKontrakTreatyKeluarXOLCommandValidator()
        {
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(HaveDetail).WithMessage("Detail Wajib diisi");
            
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(ShareMust100).WithMessage("Total share detail harus 100%");
        }
        
        private bool HaveDetail(List<DetailKontrakTreatyKeluarXOLDataDto> details)
        {
            return details.Any();
        }
        
        private bool ShareMust100(List<DetailKontrakTreatyKeluarXOLDataDto> details)
        {
            return details.Sum(s => s.pst_share) == 100;
        }
    }
}