namespace ABB.Domain.Entities
{
    public class KasBank
    {
        public string Kode { get; set; }
        public string Keterangan { get; set; }
        public string NoRekening { get; set; }
        public string NoPerkiraan { get; set; }
        public string TipeKasBank { get; set; }
        public decimal? Saldo { get; set; }
        public string KodeCabang { get; set; }
    }
}