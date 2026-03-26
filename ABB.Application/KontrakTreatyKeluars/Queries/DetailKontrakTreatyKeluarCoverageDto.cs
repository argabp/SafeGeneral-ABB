namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class DetailKontrakTreatyKeluarCoverageDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_cvrg { get; set; }

        public string nm_cvrg { get; set; }

        public decimal pst_kms_reas { get; set; }

        public decimal? max_limit_jktb { get; set; }
        
        public decimal? max_limit_prov { get; set; }
    }
}