namespace ABB.Domain.Entities
{
    public class Kapal
    {
        public string kd_kapal { get; set; }

        public string? nm_kapal { get; set; }

        public string? merk_kapal { get; set; }

        public string kd_negara { get; set; }

        public int? thn_buat { get; set; }

        public int? grt { get; set; }

        public string? st_class { get; set; }

        public string? no_reg { get; set; }

        public string? no_imo { get; set; }

        public string? ekuitas { get; set; }
    }
}