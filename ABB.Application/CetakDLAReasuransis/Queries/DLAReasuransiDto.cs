using System;

namespace ABB.Application.CetakDLAReasuransis.Queries
{
    public class DLAReasuransiDto
    {
        public string Id { get; set; }
        
        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        public string nomor_berkas { get; set; }

        public short no_dla { get; set; }
        public string nm_rk { get; set; }

        public string nm_grp_rk { get; set; }

        public string nm_ttg { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public string no_mts { get; set; }
    }
}