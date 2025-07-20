using System;

namespace ABB.Domain.Entities
{
    public class LimitAkseptasi
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }

        public decimal? pst_limit_cb { get; set; }

        public Int16? maks_panel { get; set; }
        public decimal? nilai_kapasitas { get; set; }
    }
}