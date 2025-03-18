using FluentValidation;

namespace ABB.Application.KategoriJenisKendaraans.Commands
{
    public class SaveKategoriJenisKendaraanCommandValidator : AbstractValidator<SaveKategoriJenisKendaraanCommand>
    {
        public SaveKategoriJenisKendaraanCommandValidator()
        {
            RuleFor(p => p.kd_rsk)
                .Cascade(CascadeMode.Stop)
                .Must(ThreeDigit).WithMessage("Kode Jenis Harus 3 Digit");
        }
        
        private bool ThreeDigit(string kd_rsk)
        {
            return kd_rsk.Trim().Length == 3;
        }
    }
}