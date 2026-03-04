using System;

namespace ABB.Domain.Entities
{
    public class SpLaporanPelunasanResult
    {
        public DateTime? date { get; set; }        // Tanggal Produksi
        public DateTime? tgl_byr { get; set; }     // Tanggal Lunas
        public decimal? jumlah { get; set; }       // Nilai Lunas
        public string no_nd { get; set; }          // No Nota
        public string no_pl { get; set; }          // No Polis
        public string nm_cust { get; set; }        // Customer
        public string nm_pos { get; set; }         // Pos
        public string nm_brok { get; set; }        // Broker
        public string nm_cust2 { get; set; }       // Customer 2
        public string lok { get; set; }            // Lokasi
        public string kd_tutup { get; set; }       // Kode Tutup
        public string no_bukti { get; set; }       // No Bukti
        public string jn_ass { get; set; }         // Jenis Asuransi
    }
}