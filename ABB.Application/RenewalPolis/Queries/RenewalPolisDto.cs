using System;

namespace ABB.Application.RenewalPolis.Queries
{
    public class RenewalPolisDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }
        
        public string nm_cb { get; set; }
        
        public string nm_cob { get; set; }
        
        public string nm_scob { get; set; }
        
        public byte no_renew { get; set; }

        public decimal thn_uw { get; set; }

        public string no_endt { get; set; }
        
        public DateTime tgl_input { get; set; }
        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string nm_usr_input { get; set; }
        
        public string kd_usr_input { get; set; }

        public string nm_ttg { get; set; }
    }
}