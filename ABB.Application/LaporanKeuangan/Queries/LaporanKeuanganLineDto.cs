namespace ABB.Application.LaporanKeuangan.Queries
{
    public class LaporanKeuanganLineDto
    {
        public string Deskripsi { get; set; }
        public decimal Jumlah { get; set; } // Ini angka rupiahnya
        public int Level { get; set; }      // Untuk indentasi (jorok ke dalam)
        public int Urutan { get; set; }      // Untuk indentasi (jorok ke dalam)
        public string TipeBaris { get; set; } // HEADING, DETAIL, TOTAL, SPASI
        public bool IsBold => TipeBaris == "HEADING" || TipeBaris == "TOTAL";
    }
}