using FluentValidation;

namespace ABB.Application.SebabKejadians.Commands
{
    public class SaveSebabKejadianCommandValidator : AbstractValidator<SaveSebabKejadianCommand>
    {
        public SaveSebabKejadianCommandValidator()
        {
            RuleFor(p => p.kd_sebab)
                .Cascade(CascadeMode.Stop)
                .Must(ThreeDigit).WithMessage("Kode Sebab Kejadian Harus 3 Digit");
        }
        
        private bool ThreeDigit(string kd_sebab)
        {
            return kd_sebab.Trim().Length == 3;
        }
    }
}