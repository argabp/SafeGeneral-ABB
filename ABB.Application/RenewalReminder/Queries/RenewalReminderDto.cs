using System;

namespace ABB.Application.RenewalReminder.Queries
{
    public class RenewalReminderDto
    {
        public string? kd_cb { get; set; }

        public string? kd_cob { get; set; }

        public string? kd_scob { get; set; }

        public string? kd_thn { get; set; }

        public string? no_pol { get; set; }

        public Int16? no_updt { get; set; }

        public string? nm_cob { get; set; }
        
        public string? nm_scob { get; set; }

        public string? nm_cb { get; set; }

        public string? kt_cb { get; set; }
        
        public string? nm_ttg { get; set; }
        
        public string? almt_ttg { get; set; }
        
        public string? kt_ttg { get; set; }

        public Int16? jk_wkt_ptg { get; set; }

        public DateTime? tgl_mul_ptg_ind { get; set; }
        
        public DateTime? tgl_akh_ptg_ind { get; set; }

        public string? tgl_renew { get; set; }
        
        public string? tgl_print { get; set; }

        public decimal? nilai_ptg { get; set; }
        
        public decimal? nilai_prm { get; set; }
        
        public Int16? no_renew { get; set; }
        
        public string? no_pol_ttg { get; set; }
        
        public string? kd_class { get; set; }
        
        public string? periode { get; set; }
    }
}