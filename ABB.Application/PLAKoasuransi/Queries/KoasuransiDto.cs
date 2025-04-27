using System;

namespace ABB.Application.PLAKoasuransi.Queries
{
    public class KoasuransiDto
    {
        public string Id { get; set; }
        public string nm_cb { get; set; }
        
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public Int16 no_pla { get; set; }
        public string st_tipe_pla { get; set; }
    }
}