using System;

namespace ABB.Application.RenewalNotices.Queries
{
    public class RenewalNoticeDto
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public byte? no_renew { get; set; }

        public string? nm_scob { get; set; }

        public string? nm_cb { get; set; }

        public string? kt_cb { get; set; }

        public string? nm_ttg { get; set; }

        public string? almt_ttg { get; set; }

        public string? kt_ttg { get; set; }

        public Int16 jk_wkt_ptg { get; set; }

        public string? tgl_mul_ptg_ind { get; set; }

        public string? tgl_akh_ptg_ind { get; set; }

        public string? tgl_renew { get; set; }

        public string? tgl_print { get; set; }

        public string? desk_oby_01 { get; set; }

        public decimal? nilai_oby_01 { get; set; }

        public string? desk_oby_02 { get; set; }

        public decimal? nilai_oby_02 { get; set; }

        public string? desk_oby_03 { get; set; }

        public decimal? nilai_oby_03 { get; set; }

        public string? desk_oby_04 { get; set; }

        public decimal? nilai_oby_04 { get; set; }

        public string? desk_oby_05 { get; set; }

        public decimal? nilai_oby_05 { get; set; }

        public decimal? nilai_prm { get; set; }

        public decimal? nilai_bia_pol { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public string? nm_okup { get; set; }

        public string? almt_rsk { get; set; }

        public string? ket_oby { get; set; }

        public string? no_pol_ttg { get; set; }
    }
}