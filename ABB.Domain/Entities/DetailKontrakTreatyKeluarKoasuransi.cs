using System;

namespace ABB.Domain.Entities
{
    public class DetailKontrakTreatyKeluarKoasuransi
    {
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_pps { get; set; }

        public int no_urut { get; set; }

        public decimal pst_share_mul { get; set; }

        public decimal pst_share_akh { get; set; }
        
        public decimal pst_bts_koas { get; set; }
    }
}