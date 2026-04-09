using System;

namespace ABB.Application.ProsesAlokasiKlaimReasuransis.Queries
{
    public class ProsesAlokasiKlaimReasuransiDto
    {
        public string Id { get; set; }
        
        public string nm_cb { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string nomor_register { get; set; }

        public string tipe_mts { get; set; }

        public string nm_tipe_mts { get; set; }

        public string nm_ttg { get; set; }

        public DateTime tgl_mts { get; set; }

        public string? no_pol_lama { get; set; }

        public DateTime? tgl_closing { get; set; }

        public DateTime? tgl_reas { get; set; }

        public decimal nilai_ttl_kl { get; set; }

        public decimal nilai_total_klaim { get; set; }

        public decimal sisa_alokasi { get; set; }

        public string? flag_reas { get; set; }
    }
}