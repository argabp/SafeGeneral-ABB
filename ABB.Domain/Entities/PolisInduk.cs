using System;

namespace ABB.Domain.Entities
{
    public class PolisInduk
    {
        public string no_pol_induk { get; set; }

        public string st_pol { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public decimal thn_uw { get; set; }

        public string kd_grp_ttg { get; set; }
        
        public string kd_rk_ttg { get; set; }

        public string nm_ttg { get; set; }

        public string? almt_ttg { get; set; }

        public string? kt_ttg { get; set; }

        public string? nm_qq { get; set; }

        public string? kd_grp_brk { get; set; }

        public string? kd_rk_brk { get; set; }

        public string st_pas { get; set; }

        public string? kd_grp_pas { get; set; }

        public string? kd_rk_pas { get; set; }

        public string? kd_grp_bank { get; set; }

        public string? kd_rk_bank { get; set; }

        public string kd_pkk_sb_bis { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }

        public decimal pst_share_bgu { get; set; }

        public Int16 jk_wkt_ptg { get; set; }

        public decimal faktor_prd { get; set; }

        public decimal pst_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_dis { get; set; }

        public decimal pst_kms { get; set; }

        public decimal pst_insentif { get; set; }

        public decimal nilai_min_prm { get; set; }

        public string? no_pol_pas { get; set; }

        public string? ctt_pol { get; set; }

        public string? lamp_pol { get; set; }

        public string? ket_endt { get; set; }

        public string? desk_deduct { get; set; }

        public string? ket_klausula { get; set; }

        public string flag_konv { get; set; }

        public DateTime? tgl_ttd { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime? tgl_updt { get; set; }

        public string? kd_usr_updt { get; set; }

        public string? kd_grp_mkt { get; set; }

        public string? kd_rk_mkt { get; set; }

        public decimal? nilai_deposit { get; set; }

        public decimal? nilai_tsi { get; set; }

        public byte? wpc { get; set; }

        public string? kd_mtu { get; set; }
    }
}