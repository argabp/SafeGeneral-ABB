using System;

namespace ABB.Application.CancelPostingPolis.Queries
{
    public class CancelPostingPolisDto
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public string nm_ttg { get; set; }

        public string flag_posting { get; set; }

        public DateTime? tgl_closing { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_usr_posting { get; set; }
    }
}