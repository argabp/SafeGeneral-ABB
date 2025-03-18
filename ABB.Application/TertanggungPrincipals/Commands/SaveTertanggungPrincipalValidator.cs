using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveTertanggungPrincipalValidator : AbstractValidator<SaveTertanggungPrincipalCommand>
    {
        public SaveTertanggungPrincipalValidator()
        {
            RuleFor(p => p.kd_cb)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kode Cabang Wajib Diisi");
            
            RuleFor(p => p.kd_grp_rk)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kode Group Rekanan Wajib Diisi");
            
            RuleFor(p => p.nm_rk)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nama Rekanan Wajib Diisi");
            
            RuleFor(p => p.kd_kota)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kode Kota Wajib Diisi");
            
            RuleFor(p => p.almt)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Alamat Wajib Diisi");
            
            RuleFor(p => p.flag_sic)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Retail/Corporate Wajib Diisi");
        }
    }
}