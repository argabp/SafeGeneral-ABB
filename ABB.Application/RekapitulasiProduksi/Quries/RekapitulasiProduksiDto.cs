using System;

namespace ABB.Application.RekapitulasiProduksi.Quries
{
    public class RekapitulasiProduksiDto
    {
        public string jns_nt_msk { get; set; }
        
        public string? nm_cb { get; set; }
        
        public string? nm_cob { get; set; }
        
        public decimal? nilai_prm { get; set; }
        
        public decimal? nilai_diskon { get; set; }
        
        public decimal? nilai_kms { get; set; }
        
        public decimal? nilai_net { get; set; }
        
        public decimal? nilai_net_prev { get; set; }
        
        public decimal? nilai_bia_pol { get; set; }
        
        public decimal? nilai_bia_mat { get; set; }
        
        public decimal? nilai_kms_broker { get; set; }
        
        public string kd_bln { get; set; }
        
        public string kd_thn { get; set; }
        
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public int jml_polis { get; set; }
    }
}