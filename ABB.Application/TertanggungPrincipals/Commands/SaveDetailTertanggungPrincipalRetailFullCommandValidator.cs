using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailTertanggungPrincipalRetailFullCommandValidator : AbstractValidator<SaveDetailTertanggungPrincipalRetailFullCommand>
    {
        public SaveDetailTertanggungPrincipalRetailFullCommandValidator()
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
            
            RuleFor(p => p.ktp_normh)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("No Wajib Diisi");
            
            RuleFor(p => p.ktp_rtrw)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.kodepos)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.telp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Telp Wajib Diisi");
            
            RuleFor(p => p.hp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("HP Wajib Diisi");
            
            RuleFor(p => p.wniwna)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Kewarganegaraan Wajib Terlampir");
            
            RuleFor(p => p.wniflag)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Wajib Diisi");
            
            RuleFor(p => p.npwp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("NPWP Wajib Diisi");
            
            RuleFor(p => p.kawinflag)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Status Perkawinan Wajib Diisi");
            
            RuleFor(p => p.pekerjaanflag)
                .Cascade(CascadeMode.Stop)
                .Must(IsFill).WithMessage("Pekerjaan Wajib Disi");
            
            RuleFor(p => p.pekerjaanlain)
                .Cascade(CascadeMode.Stop)
                .Must((model, pekerjaanlain) => PekerjaanDetailFill(model.pekerjaanflag, pekerjaanlain))
                .WithMessage("Wajib Diisi");
            
            RuleFor(p => p.jabatan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Jabatan Wajib Diisi");
            
            RuleFor(p => p.usaha)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Bidang Usaha Wajib Diisi");
            
            RuleFor(p => p.usahathn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.usahabln)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.usahaalamat)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Alamat Usaha Wajib Diisi");
            
            RuleFor(p => p.usahakota)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Kota Usaha Wajib Diisi");
            
            RuleFor(p => p.usahakodepos)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.usahatelp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.usahatelpext)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.usahahasilflag)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Penghasilan Wajib Diisi");
            
            RuleFor(p => p.usahaflag)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Sumber Penghasilan / Dana Wajib Diisi");
        }
    
        private bool IsFill(string? value)
        {
            return value == "1";
        }

        private bool PekerjaanDetailFill(string? pekerjaan, string? pekerjaanlain)
        {
            if (pekerjaan == "4")
            {
                return !string.IsNullOrWhiteSpace(pekerjaanlain);
            }

            return true;
        }
    }
}