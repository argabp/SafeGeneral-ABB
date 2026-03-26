using System;

namespace ABB.Domain.Entities
{
    public class DetailKontrakTreatyKeluar
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public string kd_grp_pas { get; set; }

        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }

        public decimal pst_com { get; set; }

        public string? kd_grp_sb_bis { get; set; }

        public string? kd_rk_sb_bis { get; set; }
    }
}