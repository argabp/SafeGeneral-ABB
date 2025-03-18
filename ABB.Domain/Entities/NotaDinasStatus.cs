using System;

namespace ABB.Domain.Entities
{
    public class NotaDinasStatus
    {
        public int id_nds { get; set; }

        public Int16 no_urut { get; set; }

        public string kd_user { get; set; }

        public DateTime tgl_status { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user_status { get; set; }

        public string keterangan { get; set; }
    }
}