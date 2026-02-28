using System;

namespace ABB.Application.DLAReasuransis.Queries
{
    public class DLAReasuransiDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nomor_register { get; set; }
        
        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public short no_dla { get; set; }

        public string kd_grp_pas { get; set; }

        public string nm_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public string nm_rk_pas { get; set; }

        public string? ket_dla { get; set; }
    }
}