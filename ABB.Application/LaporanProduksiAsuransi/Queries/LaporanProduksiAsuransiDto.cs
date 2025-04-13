using System;

namespace ABB.Application.LaporanProduksiAsuransi.Queries
{
    public class LaporanProduksiAsuransiDto
    {
        public string? kd_cb { get; set; }

        public string? kd_cob { get; set; }

        public string? kd_scob { get; set; }

        public string? kd_thn { get; set; }

        public string? no_pol { get; set; }

        public Int16? no_updt { get; set; }

        public string? jns_tr { get; set; }

        public string? jns_nt_msk { get; set; }

        public string? kd_bln { get; set; }

        public string? no_nt_msk { get; set; }

        public string? jns_nt_kel { get; set; }

        public string? no_nt_kel { get; set; }

        public DateTime? tgl_nt { get; set; }

        public string? nm_cb { get; set; }

        public string? nm_ttg { get; set; }

        public string? nm_qq { get; set; }

        public string? tgl_mul_ptg_ind { get; set; }

        public string? tgl_akh_ptg_ind { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }

        public string? kd_cvrg { get; set; }

        public string? kd_mtu { get; set; }

        public string? kd_symbol_mtu { get; set; }

        public decimal? nilai_prm { get; set; }

        public decimal? nilai_prm_idr { get; set; }

        public decimal? nilai_dis { get; set; }

        public decimal? nilai_dis_idr { get; set; }

        public decimal? nilai_kms { get; set; }

        public decimal? nilai_kms_idr { get; set; }

        public decimal? nilai_bia_pol { get; set; }

        public decimal? nilai_bia_pol_idr { get; set; }

        public decimal? nilai_bia_mat { get; set; }

        public decimal? nilai_bia_mat_idr { get; set; }

        public decimal? nilai_bia_lain { get; set; }

        public decimal? nilai_bia_lain_idr { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }

        public string? nm_sb_bis { get; set; }

        public string? nm_cob { get; set; }

        public string? nm_scob { get; set; }

        public string? st_pas { get; set; }

        public string? kt { get; set; }

        public string? nm_mkt { get; set; }

        public string tgl_period { get; set; }

        public string no_pol_ttg { get; set; }

        public string no_nota { get; set; }

        public string no_reg { get; set; }
    }
}