using FluentValidation;

namespace ABB.Application.TertanggungPrincipals.Commands
{
    public class SaveDetailSlikCorporateCommandValidator : AbstractValidator<SaveDetailSlikCorporateCommand>
    {
        public SaveDetailSlikCorporateCommandValidator()
        {
            RuleFor(p => p.kd_usaha2)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tmpt_pendirian)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.akta_pendirian)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tgl_akta_pendirian)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.akta_berubah_takhir)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tgl_akta_berubah_takhir)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.kd_negara)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.kd_usaha)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.kd_hub)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.langgar_bmpk_pd_pp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.lampaui_bmpk_pd_pp)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.kd_gol_deb)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.lembaga_peringkat)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tgl_pemeringkatan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.grp_usaha_deb)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.id_pengurus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.kd_jns_id_pengurus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.nm_pengurus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Jenis_Kelamin)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Alamat)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Kelurahan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Kecamatan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Kabupaten)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Kode_Jabatan)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.sts_pengurus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tgl_lap_keu_debitur)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");
            
            RuleFor(p => p.no_rek)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

        }
    }
}