using System;

namespace ABB.Application.Okupasis.Queries
{
    public class DetailOkupasiDto
    {
        public string Id { get; set; }
        
        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public string text_kls_konstr { get; set; }

        public byte stn_rate_prm { get; set; }

        public string text_stn_rate_premi { get; set; }

        public decimal pst_rate_prm { get; set; }
    }
}