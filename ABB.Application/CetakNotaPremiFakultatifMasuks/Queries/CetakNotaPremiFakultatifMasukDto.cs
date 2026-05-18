using System;

namespace ABB.Application.CetakNotaPremiFakultatifMasuks.Queries
{
    public class CetakNotaPremiFakultatifMasukDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }

        public string nomor_akseptasi { get; set; }

        public string no_pol_ttg_masked { get; set; }

        public DateTime tgl_closing { get; set; }

        public string nm_ttg { get; set; }
    }
}