using System;

namespace ABB.Domain.Entities
{
    public class LimitKlaim
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string nm_limit { get; set; }
    }
}