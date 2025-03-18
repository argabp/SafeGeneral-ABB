using FluentValidation;

namespace ABB.Application.Lokasis.Commands
{
    public class SaveKabupatenCommandValidator : AbstractValidator<SaveKabupatenCommand>
    {
        public SaveKabupatenCommandValidator()
        {
            RuleFor(p => p.kd_kab)
                .Cascade(CascadeMode.Stop)
                .Must(ThreeDigit).WithMessage("Kode Kabupaten Harus 3 Digit");
        }
        
        private bool ThreeDigit(string kd_kab)
        {
            return kd_kab.Trim().Length == 3;
        }
    }
}