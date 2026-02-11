using System;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class KeteranganProduksiDto
    {
        public long Id { get; set; }          // Primary Key
        public long IdNota { get; set; }      // Relasi ke Produksi (optional kalau ada)
        public string NoNota { get; set; }    // Nomor Nota
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
    }
}
