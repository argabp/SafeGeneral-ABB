using System.Collections.Generic;
using System.Linq;
using ABB.Application.NotaFakultatifKeluars.Queries;
using FluentValidation;

namespace ABB.Application.NotaFakultatifKeluars.Commands
{
    public class SaveNotaFakultatifKeluarCommandValidator : AbstractValidator<SaveNotaFakultatifKeluarCommand>
    {
        public SaveNotaFakultatifKeluarCommandValidator()
        {
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(HaveDetail).WithMessage("Detail Wajib diisi");
            
            RuleFor(p => p.Details)
                .Cascade(CascadeMode.Stop)
                .Must(AngsuranMust100).WithMessage("Total angsuran detail harus 100%");
        }
        
        private bool HaveDetail(List<DetailNotaFakultatifKeluarDto> details)
        {
            return details.Any();
        }
        
        private bool AngsuranMust100(List<DetailNotaFakultatifKeluarDto> details)
        {
            return details.Sum(s => s.pst_ang) == 100;
        }
    }
}