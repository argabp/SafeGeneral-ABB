using System;

namespace ABB.Application.BukuKerugian.Queries
{
    public class BukuKerugianDto
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string? nm_cb { get; set; }

        public DateTime? tgl_mul { get; set; }
        
        public DateTime? tgl_akh { get; set; }

        public string? nm_cob { get; set; }

        public string? kd_mtu { get; set; }

        public string? no_berkas { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? no_sert { get; set; }

        public string? nm_ttg { get; set; }

        public string? nm_oby { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? tempat_kej { get; set; }

        public DateTime? tgl_mul_ptg { get; set; }
        
        public DateTime? tgl_akh_ptg { get; set; }

        public DateTime? tgl_kej { get; set; }

        public decimal? nilai_share_bgu { get; set; }

        public decimal? nilai_share_bgu_idr { get; set; }

        public decimal? pst_share_bgu { get; set; }

        public string? kd_mtu_symbol_tsi { get; set; }

        public decimal? nilai_ttl_kl { get; set; }

        public decimal? nilai_ttl_kl_idr { get; set; }

        public string? nm_sifat_kerugian { get; set; }

        public string? nm_mtu { get; set; }

        public string? kd_mtu_symbol { get; set; }
    }
}