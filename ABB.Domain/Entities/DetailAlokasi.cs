using System;

namespace ABB.Domain.Entities
{
    public class DetailAlokasi
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_updt_reas { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_grp_sor { get; set; }

        public string kd_rk_sor { get; set; }

        public decimal pst_share { get; set; }

        public decimal nilai_prm_reas { get; set; }

        public decimal pst_adj_reas { get; set; }

        public byte stn_adj_reas { get; set; }

        public decimal nilai_adj_reas { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal nilai_kms_reas { get; set; }

        public decimal pst_pjk_reas { get; set; }

        public decimal nilai_pjk_reas { get; set; }

        public decimal nilai_ttl_ptg_reas { get; set; }

        public string? no_pol_ttg { get; set; }

        public string kd_grp_sb_bis { get; set; }
        public string kd_rk_sb_bis { get; set; }
    }
}