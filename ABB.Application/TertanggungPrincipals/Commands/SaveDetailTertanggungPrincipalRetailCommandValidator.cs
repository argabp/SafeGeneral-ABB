using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailTertanggungPrincipalRetailCommandValidator : AbstractValidator<SaveDetailTertanggungPrincipalRetailCommand>
    {
        public SaveDetailTertanggungPrincipalRetailCommandValidator()
        {
            RuleFor(p => p.ktp_nm)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Nama Lengkap Wajib Diisi");
            
            RuleFor(p => p.ktp_tempat)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tempat Lahir Wajib Diisi");
            
            RuleFor(p => p.ktp_tgl)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tanggal Lahir Wajib Diisi");
            
            RuleFor(p => p.kelamin)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Jenis Kelamin Wajib Diisi");
            
            RuleFor(p => p.ktp_no)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("No. KTP SIM / Paspor Wajib Diisi");
            
            RuleFor(p => p.ktp_alamat)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Alamat Wajib Diisi");
            
            RuleFor(p => p.ktp_kota)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kota Wajib Diisi");
            
            RuleFor(p => p.kodepos)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kodepos Wajib Diisi");
        }
    }
}