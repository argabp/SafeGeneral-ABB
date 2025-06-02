using System;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class ReportPengajuanAkseptasiDto
    {
        public string? kd_cb { get; set; }

        public string? kd_cob { get; set; }

        public string? kd_scob { get; set; }

        public string? kd_thn { get; set; }

        public string? no_aks { get; set; }

        public string? nomor_pengajuan { get; set; }

        public string? nm_cb { get; set; }

        public string? nm_cob { get; set; }

        public string? nm_scob { get; set; }

        public string? nm_mkt { get; set; }

        public string? nm_sb_bis { get; set; }

        public string? nm_ttg { get; set; }

        public DateTime? tgl_pengajuan { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }

        public string? symbol { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal? pst_kms { get; set; }

        public string? st_pas { get; set; }

        public decimal? pst_share { get; set; }

        public string? no_pol_pas { get; set; }

        public string? nm_pas1 { get; set; }

        public decimal? pst_pas1 { get; set; }

        public string? nm_pas2 { get; set; }

        public decimal? pst_pas2 { get; set; }

        public string? nm_pas3 { get; set; }

        public decimal? pst_pas3 { get; set; }

        public string? nm_pas4 { get; set; }

        public decimal? pst_pas4 { get; set; }

        public string? nm_pas5 { get; set; }

        public decimal? pst_pas5 { get; set; }

        public string? ket_rsk { get; set; }

        public string? ttd_dibuat { get; set; }

        public string? jabatan_dibuat { get; set; }

        public string? nm_dibuat { get; set; }

        public DateTime? tgl_dibuat { get; set; }

        public string? user_id_dibuat { get; set; }

        public string? ttd_diperiksa { get; set; }

        public string? jabatan_diperiksa { get; set; }

        public string? nm_diperiksa { get; set; }

        public DateTime? tgl_diperiksa { get; set; }

        public string? user_id_diperiksa { get; set; }

        public string? ttd_disetujui { get; set; }

        public string? jabatan_disetujui { get; set; }

        public string? nm_disetujui { get; set; }

        public DateTime? tgl_disetujui { get; set; }

        public string? user_id_disetujui { get; set; }
    }
}