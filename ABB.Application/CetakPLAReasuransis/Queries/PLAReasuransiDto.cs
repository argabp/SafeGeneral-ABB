using System;

namespace ABB.Application.CetakPLAReasuransis.Queries
{
    public class PLAReasuransiDto
    {
        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        public string nomor_pla { get; set; }
        public string nm_rk { get; set; }

        public string nm_grp_rk { get; set; }

        public string nm_ttg { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
    }
}