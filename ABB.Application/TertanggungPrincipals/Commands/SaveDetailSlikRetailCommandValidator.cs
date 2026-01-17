using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailSlikRetailCommandValidator : AbstractValidator<SaveDetailSlikRetailCommand>
    {
        public SaveDetailSlikRetailCommandValidator()
        {
            RuleFor(p => p.kd_st_pndk_glr)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kd_negara)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kd_pekerjaan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kd_usaha)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kd_hub)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kd_gol_deb)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.langgar_bmpk_pd_pp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.lampaui_bmpk_pd_pp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.nm_ibu_kdg)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.no_rek)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
        }
    }
}