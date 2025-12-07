using System;

namespace ABB.Domain.Entities
{
    public class KlaimStatusAttachment
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }
        
        public Int16 no_urut { get; set; }

        public Int16 no_dokumen { get; set; }

        public string nm_dokumen { get; set; }
    }
}