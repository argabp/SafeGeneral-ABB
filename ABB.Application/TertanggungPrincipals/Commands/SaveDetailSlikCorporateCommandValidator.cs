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

            RuleFor(p => p.pangsa_kepemilikan)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.sts_pengurus)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.tgl_lap_keu_debitur)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Wajib Diisi");

            RuleFor(p => p.Aset)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Aset_Lancar)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Kas_n_Setara_Kas)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Piutang_Usaha_atau_Pembiayaan)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Inv_atau_Aset_Keu_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Aset_Lancar_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Aset_Tak_Lancar)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Piut_Usaha_atau_Pembiayaan)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Inv_atau_Aset_Keu_Lain2)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Aset_Tak_lancar_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Liabilitas)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Liabilitas_Jgk_Pdk)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Pinjam_Jgk_Pdk)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Utang_Jgk_Pdk)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Liabilitas_Jgk_Pdk_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Liabilitas_Jgk_Pjg)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Pinjaman_Jgk_Pjg)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Utang_Jgk_Pjg)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Liabilitas_Jgk_Pjg_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Ekuitas)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Pendapatan)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Beban_Pkk_Pendapatan)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.L_atau_R_Bruto)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Pendapatan_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.Beban_Lain)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.L_atau_R_belum_Pjk)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

            RuleFor(p => p.L_atau_R_Thn_Bjl)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Wajib Diisi");

        }
    }
}