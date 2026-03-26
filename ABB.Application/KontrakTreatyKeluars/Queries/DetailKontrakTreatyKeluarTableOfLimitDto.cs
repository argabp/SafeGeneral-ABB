namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class DetailKontrakTreatyKeluarTableOfLimitDto
    {
        public string Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_okup { get; set; }

        public string nm_okup { get; set; }

        public string category_rsk { get; set; }
        
        public string nm_category_rsk { get; set; }

        public string kd_kls_konstr { get; set; }

        public string nm_kls_konstr { get; set; }
        
        public decimal pst_bts_tty { get; set; }
    }
}