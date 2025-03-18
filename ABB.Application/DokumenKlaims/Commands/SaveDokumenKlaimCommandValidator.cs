using FluentValidation;

namespace ABB.Application.DokumenKlaims.Commands
{
    public class SaveDokumenKlaimCommandValidator : AbstractValidator<SaveDokumenKlaimCommand>
    {
        public SaveDokumenKlaimCommandValidator()
        {
            RuleFor(p => p.kd_dok)
                .Cascade(CascadeMode.Stop)
                .Must(TwoDigit).WithMessage("Kode Dokumen Klaim Harus 2 Digit");
        }
        
        private bool TwoDigit(string kd_dok)
        {
            return kd_dok.Trim().Length == 2;
        }
    }
}