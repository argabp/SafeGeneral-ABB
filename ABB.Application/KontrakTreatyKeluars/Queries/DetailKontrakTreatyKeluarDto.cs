namespace ABB.Application.KontrakTreatyKeluars.Queries
{
    public class DetailKontrakTreatyKeluarDto
    {
        public string Id { get; set; }

        public string nm_grp_sb_bis { get; set; }
        
        public string nm_rk_sb_bis { get; set; }

        public string? nm_grp_pas { get; set; }
        
        public string? nm_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal pst_com { get; set; }

        public string kd_grp_pas { get; set; }
        public string kd_rk_pas { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }
    }
}