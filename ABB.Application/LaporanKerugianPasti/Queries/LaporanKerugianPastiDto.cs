using System;

namespace ABB.Application.LaporanKerugianPasti.Queries
{
    public class LaporanKerugianPastiDto
    {
        public string no_berkas { get; set; }

        public string nm_ttg { get; set; }
        
        public string no_pol_ttg { get; set; }
        
        public string nm_scob { get; set; }
        
        public string nm_oby { get; set; }
        
        public string symbol_ptg { get; set; }
        
        public string? symbol { get; set; }
        public string? symbol_estimasi_pla { get; set; }
        
        public decimal nilai_ttl_ptg { get; set; }
        
        public decimal nilai_share_bgu { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public DateTime tgl_kej { get; set; }

        public string tempat_kej { get; set; }

        public string kond_ptg { get; set; }

        public string ket_oby { get; set; }

        public string sebab_kerugian { get; set; }

        public string nm_sifat_kerugian { get; set; }

        public decimal nilai_ttl_kl { get; set; }

        public string symbol_kl { get; set; }

        public DateTime tgl_lns_prm { get; set; }

        public string no_bukti_lns { get; set; }

        public string validitas { get; set; }

        public string kt_cb { get; set; }

        public string tgl_closing_ind { get; set; }

        public string no_berkas_pla { get; set; }

        public decimal nilai_estimasi { get; set; }

        public DateTime tgl_closing { get; set; }

        public string nm_oby_1 { get; set; }

        public decimal nilai_kl_1 { get; set; }

        public string nm_oby_2 { get; set; }

        public decimal nilai_kl_2 { get; set; }

        public string nm_oby_3 { get; set; }

        public decimal nilai_kl_3 { get; set; }

        public string nm_oby_4 { get; set; }

        public decimal nilai_kl_4 { get; set; }

        public string nm_oby_5 { get; set; }

        public decimal nilai_kl_5 { get; set; }

        public string nm_oby_6 { get; set; }

        public decimal nilai_kl_6 { get; set; }

        public string ket_kl { get; set; }
        public string ket_jns { get; set; }
        public string kd_cob { get; set; }
    }
}