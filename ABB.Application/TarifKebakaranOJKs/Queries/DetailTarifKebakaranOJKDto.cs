namespace ABB.Application.TarifKebakaranOJKs.Queries
{
    public class DetailTarifKebakaranOJKDto
    {
        public int Id { get; set; }
        
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public string text_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public string text_stn_rate_premi { get; set; }

        public decimal pst_rate_prm_min { get; set; }
        public decimal pst_rate_prm_max { get; set; }
    }
}