using System;

namespace ABB.Application.TertanggungPrincipals.Queries
{
    public class DetailSlikDto
    {
        public string kd_cb { get; set; }
        public string kd_grp_rk { get; set; }
        public string kd_rk { get; set; }
        public string kd_st_pndk_glr { get; set; }
        public string? email { get; set; }
        public string kd_negara { get; set; }
        public string kd_pekerjaan { get; set; }
        public string? tmpt_kerja { get; set; }
        public string kd_usaha { get; set; }
        public string? almt_kerja { get; set; }
        public decimal? pdb_thn { get; set; }
        public string? kd_hasil { get; set; }
        public short? tanggungan { get; set; }
        public string kd_hub { get; set; }
        public string kd_gol_deb { get; set; }
        public string? id_pasangan { get; set; }
        public string? nm_pasangan { get; set; }
        public DateTime? tgl_lhr_pasangan { get; set; }
        public string? pisah_harta { get; set; }
        public string langgar_bmpk_pd_pp { get; set; }
        public string lampaui_bmpk_pd_pp { get; set; }
        public string nm_ibu_kdg { get; set; }
        public string kd_usaha2 { get; set; }
        public string tmpt_pendirian { get; set; }
        public string akta_pendirian { get; set; }
        public DateTime tgl_akta_pendirian { get; set; }
        public string akta_berubah_takhir { get; set; }
        public DateTime tgl_akta_berubah_takhir { get; set; }
        public string? rating_debitur { get; set; }
        public string lembaga_peringkat { get; set; }
        public DateTime tgl_pemeringkatan { get; set; }
        public string grp_usaha_deb { get; set; }
        public string id_pengurus { get; set; }
        public string kd_jns_id_pengurus { get; set; }
        public string nm_pengurus { get; set; }
        public string Jenis_Kelamin { get; set; }
        public string Alamat { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string Kabupaten { get; set; }
        public string Kode_Jabatan { get; set; }
        public decimal pangsa_kepemilikan { get; set; }
        public string sts_pengurus { get; set; }
        public DateTime tgl_lap_keu_debitur { get; set; }
        public decimal Aset { get; set; }
        public decimal Aset_Lancar { get; set; }
        public decimal Kas_n_Setara_Kas { get; set; }
        public decimal Piutang_Usaha_atau_Pembiayaan { get; set; }
        public decimal Inv_atau_Aset_Keu_Lain { get; set; }
        public decimal Aset_Lancar_Lain { get; set; }
        public decimal Aset_Tak_Lancar { get; set; }
        public decimal Piut_Usaha_atau_Pembiayaan { get; set; }
        public decimal Inv_atau_Aset_Keu_Lain2 { get; set; }
        public decimal Aset_Tak_lancar_Lain { get; set; }
        public decimal Liabilitas { get; set; }
        public decimal Liabilitas_Jgk_Pdk { get; set; }
        public decimal Pinjam_Jgk_Pdk { get; set; }
        public decimal Utang_Jgk_Pdk { get; set; }
        public decimal Liabilitas_Jgk_Pdk_Lain { get; set; }
        public decimal Liabilitas_Jgk_Pjg { get; set; }
        public decimal Pinjaman_Jgk_Pjg { get; set; }
        public decimal Utang_Jgk_Pjg { get; set; }
        public decimal Liabilitas_Jgk_Pjg_Lain { get; set; }
        public decimal Ekuitas { get; set; }
        public decimal Pendapatan { get; set; }
        public decimal Beban_Pkk_Pendapatan { get; set; }
        public decimal L_atau_R_Bruto { get; set; }
        public decimal Pendapatan_Lain { get; set; }
        public decimal Beban_Lain { get; set; }
        public decimal L_atau_R_belum_Pjk { get; set; }
        public decimal L_atau_R_Thn_Bjl { get; set; }
    }
}