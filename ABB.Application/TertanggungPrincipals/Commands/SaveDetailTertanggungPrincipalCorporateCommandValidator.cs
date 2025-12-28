using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailTertanggungPrincipalCorporateCommandValidator : AbstractValidator<SaveDetailTertanggungPrincipalCorporateCommand>
    {
        public SaveDetailTertanggungPrincipalCorporateCommandValidator()
        {
            RuleFor(p => p.perusahaaninstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nama Institusi/Perusahaan Wajib Diisi");
            
            RuleFor(p => p.npwp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("NPWP Wajib Diisi");
            
            RuleFor(p => p.kotainstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kota Wajib Diisi");
            
            RuleFor(p => p.kodeposinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kode Pos Wajib Diisi");
            
            RuleFor(p => p.telpinstitusi)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Telp Wajib Diisi");
        }
    }
}