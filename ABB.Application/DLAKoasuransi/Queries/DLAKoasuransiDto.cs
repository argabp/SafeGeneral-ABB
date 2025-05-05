using System;

namespace ABB.Application.DLAKoasuransi.Queries
{
    public class DLAKoasuransiDto
    {
        public string nm_ttg { get; set; }
        
        public string no_pol_ttg { get; set; }
        
        public string nm_scob { get; set; }
        
        public string nm_oby { get; set; }
        
        public string symbol { get; set; }
        
        public decimal nilai_ttl_ptg { get; set; }
        
        public decimal nilai_share_bgu { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public DateTime tgl_kej { get; set; }

        public string sebab_kerugian { get; set; }

        public decimal nilai_ttl_kl { get; set; }
        
        public string ket_dia { get; set; }

        public string nm_jns_sor_01 { get; set; }

        public decimal? nilai_jns_sor_01 { get; set; }

        public string nm_jns_sor_02 { get; set; }

        public decimal? nilai_jns_sor_02 { get; set; }

        public string no_sert { get; set; }

        public string kt_cb { get; set; }

        public DateTime tgl_closing_ind { get; set; }

        public string no_berkas_reas { get; set; }

        public string tempat_kej { get; set; }

        public string sifat_kerugian { get; set; }

        public decimal pst_share { get; set; }

        public string no_kl { get; set; }

        public Int16 no_dla { get; set; }
        
        public Int16 no_mts { get; set; }

        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string nm_pas { get; set; }

        public string almt_pas { get; set; }

        public string kt_pas { get; set; }

        public decimal nilai_kl { get; set; }

        public string footer { get; set; }
    }
}