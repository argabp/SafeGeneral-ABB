using System;

namespace ABB.Application.PenyelesaianKlaimReasuransis.Queries
{
    public class PenyelesaianKlaimReasuransiDto
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

        public decimal? nilai_ttl_dla { get; set; }

        public string? nm_sifat_kerugian { get; set; }

        public string? nm_ttg { get; set; }

        public string? kd_mtu_symbol { get; set; }

        public decimal? nilai_ttl_pla { get; set; }

        public string? nm_mtu { get; set; }

        public decimal? jns_sor_qts { get; set; }
        public decimal? jns_sor_spl { get; set; }
        public decimal? jns_sor_con { get; set; }
        public decimal? jns_sor_pol { get; set; }
        public decimal? jns_sor_bppdan { get; set; }
        public decimal? jns_sor_fac { get; set; }
        public decimal? jns_sor_xol { get; set; }
        public string? no_berkas { get; set; }
        public string? jns_tr { get; set; }
        public string? jns_nt_msk { get; set; }
        public string? kd_bln { get; set; }
        public string? no_nt_msk { get; set; }
        public string? jns_nt_kel { get; set; }
        public string? no_nt_kel { get; set; }
        public DateTime? tgl_closing { get; set; }
    }
}