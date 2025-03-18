using FluentValidation;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveKecamatanCommandValidator : AbstractValidator<SaveKecamatanCommand>
    {
        public SaveKecamatanCommandValidator()
        {
            RuleFor(p => p.kd_kab)
                .Cascade(CascadeMode.Stop)
                .Must(ThreeDigit).WithMessage("Kode Kabupaten Harus 3 Digit");
            
            RuleFor(p => p.kd_kec)
                .Cascade(CascadeMode.Stop)
                .Must(FourDigit).WithMessage("Kode Kecamatan Harus 4 Digit");
        }
        
        private bool ThreeDigit(string kd_kab)
        {
            return kd_kab.Trim().Length == 3;
        }
        
        private bool FourDigit(string kd_kc)
        {
            return kd_kc.Trim().Length == 4;
        }
    }
}