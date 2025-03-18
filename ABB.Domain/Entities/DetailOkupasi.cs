using System;

namespace ABB.Domain.Entities
{
    public class DetailOkupasi
    {
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public decimal pst_rate_prm { get; set; }
    }
}