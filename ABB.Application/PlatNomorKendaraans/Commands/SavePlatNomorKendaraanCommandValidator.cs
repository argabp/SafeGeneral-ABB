using FluentValidation;

namespace ABB.Application.PlatNomorKendaraans.Commands
{
    public class SavePlatNomorKendaraanCommandValidator : AbstractValidator<SavePlatNomorKendaraanCommand>
    {
        public SavePlatNomorKendaraanCommandValidator()
        {
            RuleFor(p => p.kd_rsk)
                .Cascade(CascadeMode.Stop)
                .Must(ThreeDigit).WithMessage("Kode Plat Maksimum 3 Digit");
        }
        
        private bool ThreeDigit(string kd_rsk)
        {
            return kd_rsk.Trim().Length <= 3;
        }
    }
}