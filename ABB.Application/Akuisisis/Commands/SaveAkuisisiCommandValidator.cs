using FluentValidation;

namespace ABB.Application.Akuisisis.Commands
{
    public class SaveAkuisisiCommandValidator : AbstractValidator<SaveAkuisisiCommand>
    {
        public SaveAkuisisiCommandValidator()
        {
            RuleFor(p => p.kd_thn)
                .Cascade(CascadeMode.Stop)
                .Must(FourDigit).WithMessage("Kode Tahun Harus 4 Digit");
        }
        
        private bool FourDigit(int kd_thn)
        {
            return kd_thn.ToString().Length == 4;
        }
    }
}