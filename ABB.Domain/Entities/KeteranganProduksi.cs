using System;

namespace ABB.Domain.Entities
{
    public class KeteranganProduksi
    {
        public int Id { get; set; }
        public string Keterangan { get; set; }
        public string NoNota { get; set; }
        public DateTime? Tanggal { get; set; }
    }
}