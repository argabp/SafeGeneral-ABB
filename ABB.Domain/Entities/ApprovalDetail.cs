using System;

namespace ABB.Domain.Entities
{
    public class ApprovalDetail
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user_sign { get; set; }

        public Int16 sla { get; set; }
    }
}