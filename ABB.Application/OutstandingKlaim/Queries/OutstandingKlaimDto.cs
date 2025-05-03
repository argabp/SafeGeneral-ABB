using System;

namespace ABB.Application.OutstandingKlaim.Queries
{
    public class OutstandingKlaimDto
    {
        public string? nm_cb { get; set; }

        public string? nm_cob { get; set; }
        
        public string? kd_cb { get; set; }
        
        public string? kd_cob { get; set; }
        
        public string? kd_scob { get; set; }
        
        public string? kd_thn { get; set; }
        
        public string? no_kl { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? no_sert { get; set; }

        public string? nm_oby { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? tempat_kej { get; set; }
        
        public DateTime? tgl_mul_ptg { get; set; }
        
        public DateTime? tgl_akh_ptg { get; set; }

        public DateTime? tgl_akh { get; set; }
        
        public DateTime? tgl_kej { get; set; }
        
        public DateTime? tgl_lapor { get; set; }

        public string? kd_mtu_symbol_tsi { get; set; }

        public decimal? nilai_share_bgu { get; set; }

        public decimal? nilai_ttl_kl { get; set; }

        public string? nm_sifat_kerugian { get; set; }
    }
}