using System;

namespace ABB.Application.ListingSpreadingOfRisks.Commands
{
    public class ListingSpreadingOfRiskModel
    {
        public string nm_cob { get; set; }

        public string no_pol_ttg { get; set; }

        public string nm_ttg { get; set; }

        public string nm_scob { get; set; }

        public string tgl_closing_id { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string pst_cvrg { get; set; }

        public string almt_rsk { get; set; }

        public string nm_tol { get; set; }

        public string nm_mtu { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public decimal pst_rate_prm { get; set; }

        public string stn_rate_prm_rsk { get; set; }

        public string tgl_mul_ptg_ind { get; set; }

        public string tgl_akh_ptg_ind { get; set; }

        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public decimal nilai_prm_rsk { get; set; }

        public decimal nilai_dis_rsk { get; set; }

        public decimal nilai_prm_rsk_net { get; set; }

        public string ket_rsk { get; set; }

        public string kd_jns_sor { get; set; }

        public string nm_tty_pps { get; set; }

        public string symbol { get; set; }

        public decimal nilai_ttl_ptg_reas { get; set; }

        public decimal pst_share_reas { get; set; }

        public decimal nilai_prm_reas { get; set; }

        public decimal pst_adj_reas { get; set; }

        public decimal nilai_adj_reas { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal nilai_kms_reas { get; set; }

        public string nm_kpl_bag { get; set; }

        public string nm_bag { get; set; }
    }
}