using System;

namespace ABB.Domain.Entities
{
    public class TRAkseptasi
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public string nomor_pengajuan { get; set; }

        public string kd_grp_mkt { get; set; }

        public string kd_rk_mkt { get; set; }

        public string kd_grp_sb_bis { get; set; }

        public string kd_rk_sb_bis { get; set; }

        public string kd_grp_ttg { get; set; }

        public string kd_rk_ttg { get; set; }

        public string ket_rsk { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public string st_pas { get; set; }

        public decimal pst_share { get; set; }

        public string? no_pol_pas { get; set; }

        public string? kd_grp_pas1 { get; set; }

        public string? kd_rk_pas1 { get; set; }

        public decimal? pst_pas1 { get; set; }

        public string? kd_grp_pas2 { get; set; }

        public string? kd_rk_pas2 { get; set; }

        public decimal? pst_pas2 { get; set; }

        public string? kd_grp_pas3 { get; set; }

        public string? kd_rk_pas3 { get; set; }

        public decimal? pst_pas3 { get; set; }

        public string? kd_grp_pas4 { get; set; }

        public string? kd_rk_pas4 { get; set; }

        public decimal? pst_pas4 { get; set; }

        public string? kd_grp_pas5 { get; set; }

        public string? kd_rk_pas5 { get; set; }

        public decimal? pst_pas5 { get; set; }

        public string kd_user_status { get; set; }

        public DateTime tgl_status { get; set; }

        public Int16 kd_status { get; set; }

        public DateTime tgl_pengajuan { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_user_input { get; set; }

        public DateTime tgl_update { get; set; }

        public string kd_user_update { get; set; }

        public decimal? pst_dis { get; set; }
        
        public decimal? pst_kms { get; set; }

        public bool? flag_approved { get; set; }
        public string? flag_closing { get; set; }
    }
}