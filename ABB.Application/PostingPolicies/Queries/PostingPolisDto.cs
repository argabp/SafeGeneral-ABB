using System;

namespace ABB.Application.PostingPolicies.Queries
{
    public class PostingPolisDto
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public byte no_renew { get; set; }

        public string no_endt { get; set; }

        public string nm_ttg { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public string kd_usr_input { get; set; }

        public DateTime tgl_input { get; set; }

        public DateTime? tgl_closing { get; set; }
        public DateTime? tgl_posting { get; set; }

        public string? kd_updt { get; set; }
        public string? kd_usr_posting { get; set; }

        public string flag_posting { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? no_brdr { get; set; }
    }
}