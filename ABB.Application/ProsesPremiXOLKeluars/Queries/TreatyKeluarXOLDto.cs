using System;

namespace ABB.Application.ProsesPremiXOLKeluars.Queries
{
    public class TreatyKeluarXOLDto
    {
        public string Id { get; set; }
        
        public string nm_cob { get; set; }

        public string kd_tty_npps { get; set; }

        public string nm_tty_npps { get; set; }

        public string thn_tty_npps { get; set; }

        public string npps_layer { get; set; }

        public DateTime tgl_mul_ptg { get; set; }
        
        public DateTime tgl_akh_ptg { get; set; }

        public decimal nilai_bts_or { get; set; }

        public decimal nilai_bts_tty { get; set; }
        
        public decimal pst_adj_onrpi { get; set; }

        public string? ket_tty_npps { get; set; }
    }
}