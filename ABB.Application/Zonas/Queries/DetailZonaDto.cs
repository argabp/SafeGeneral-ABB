namespace ABB.Application.Zonas.Queries
{
    public class DetailZonaDto
    {
        public string Id { get; set; }
        
        public string kd_zona { get; set; }

        public string kd_kls_konstr { get; set; }
        
        public string nm_kls_konstr { get; set; }

        public string nm_zona_gb { get; set; }

        public string kd_okup { get; set; }

        public string nm_okup { get; set; }

        public byte stn_rate_prm { get; set; }

        public string text_stn_rate_premi { get; set; }

        public decimal pst_rate_prm { get; set; }
    }
}