using System;

namespace ABB.Application.KontrakTreatyKeluarXOLs.Queries
{
    public class KontrakTreatyKeluarXOLDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }
        
        public string kd_jns_sor { get; set; }

        public string nm_jns_sor { get; set; }
        
        public string kd_tty_npps { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_tty_npps { get; set; }

        public string npps_layer { get; set; }
        
        public string nm_tty_npps { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public decimal nilai_bts_or { get; set; }

        public decimal nilai_bts_tty { get; set; }

        public decimal pst_adj_onrpi { get; set; }

        public string? ket_tty_npps { get; set; }

        public decimal? nilai_kurs { get; set; }

        public decimal? pst_reinst { get; set; }

        public decimal? mindep { get; set; }

        public short? hit { get; set; }
    }
}