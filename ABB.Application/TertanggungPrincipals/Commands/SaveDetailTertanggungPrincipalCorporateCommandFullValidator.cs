using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailTertanggungPrincipalCorporateCommandFullValidator : AbstractValidator<SaveDetailTertanggungPrincipalCorporateFullCommand>
    {
        public SaveDetailTertanggungPrincipalCorporateCommandFullValidator()
        {
            RuleFor(p => p.perusahaaninstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nama Institusi/Perusahaan Wajib Diisi");
            
            RuleFor(p => p.npwpinstitusi)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Wajib Terlampir");
            
            RuleFor(p => p.npwp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("NPWP Wajib Diisi");
            
            RuleFor(p => p.siupinstitusi)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Wajib Terlampir");
            
            RuleFor(p => p.siup)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("SIUP Wajib Diisi");
            
            RuleFor(p => p.tdpinstitusi)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Wajib Terlampir");
            
            RuleFor(p => p.hukumhaminstitusi)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Akte Pendirian Wajib Terlampir");
            
            RuleFor(p => p.usahainstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Bidang Ysaga Wajib Diisi");
            
            RuleFor(p => p.kotainstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kota Wajib Diisi");
            
            RuleFor(p => p.kodeposinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kode Pos Wajib Diisi");
            
            RuleFor(p => p.telpinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Telp Wajib Diisi");
            
            RuleFor(p => p.telpextinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ext Wajib Diisi");
            
            RuleFor(p => p.no_fax)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("No Fax Wajib Diisi");
            
            RuleFor(p => p.website)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Website Perusahaan Wajib Diisi");
            
            RuleFor(p => p.dirinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nama Direktur Wajib Diisi");
            
            RuleFor(p => p.wniwna)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.wniflag)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
        }
    
        private bool IsFill(string? value)
        {
            return value == "1";
        }
    }
}