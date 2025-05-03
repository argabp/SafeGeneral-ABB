using System;

namespace ABB.Application.PenyelesaianKlaim.Queries
{
    public class PenyelesaianKlaimDto
    {
        
        public string? nm_cb { get; set; }

        public DateTime? tgl_mul { get; set; }
        
        public DateTime? tgl_akh { get; set; }

        public string? nm_cob { get; set; }
        
        public string? kd_mtu { get; set; }

        public string? no_berkas { get; set; }

        public string? no_nota { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? no_sert { get; set; }

        public string? nm_oby { get; set; }

        public string? sebab_kerugian { get; set; }

        public string? tempat_kej { get; set; }

        public DateTime? tgl_closing { get; set; }
        
        public DateTime? tgl_mul_ptg { get; set; }
        
        public DateTime? tgl_akh_ptg { get; set; }

        public DateTime? tgl_kej { get; set; }

        public decimal? nilai_share_bgu { get; set; }

        public decimal? nilai_ttl_pla { get; set; }

        public decimal? nilai_ttl_dla { get; set; }

        public decimal? pst_share_bgu { get; set; }

        public string? kd_mtu_symbol_tsi { get; set; }

        public string? nm_flag_settled { get; set; }

        public decimal? bia_mat { get; set; }

        public string? nm_mtu { get; set; }
    }
}