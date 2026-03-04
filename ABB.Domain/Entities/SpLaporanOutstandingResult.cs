using System;

namespace ABB.Domain.Entities
{
    public class SpLaporanOutstandingResult
    {
        public string no_nd { get; set; }
        public string no_pl { get; set; }
        public string nm_cust2 { get; set; }
        public string nm_pos { get; set; }
        public string nm_brok { get; set; }
        public string jn_ass { get; set; }
        public string lok { get; set; }
        public string kd_tutup { get; set; }
        public DateTime? date { get; set; }
        public DateTime? tgl_jth_tempo { get; set; }
        public string curensi { get; set; }
        public decimal? kurs { get; set; }
        public decimal? saldo { get; set; }  // Nilai Nota
        public decimal? jumlah { get; set; } // Nilai Total Bayar (Bank+Kas+Piutang)
    }
}