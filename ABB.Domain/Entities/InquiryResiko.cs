using System;

namespace ABB.Domain.Entities
{
    public class InquiryResiko
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string? ket_rsk { get; set; }

        public string kd_mtu_prm { get; set; }

        public decimal pst_rate_prm { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal? pst_dis { get; set; }

        public decimal nilai_dis { get; set; }

        public decimal? pst_kms { get; set; }

        public decimal nilai_kms { get; set; }

        public decimal? nilai_insentif { get; set; }

        public decimal? nilai_kl { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public Int16? jk_wkt_ptg { get; set; }

        public decimal? faktor_prd { get; set; }

        public decimal? pst_share_bgu { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_tol { get; set; }

        public string? kode { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }

        public DateTime? tgl_akh_ptg { get; set; }
    }
}