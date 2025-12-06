using System;

namespace ABB.Application.DLAKoasuransi.Queries
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
        public Int16 no_dla { get; set; }
        public string st_tipe_dla { get; set; }
        public string nomor_berkas { get; set; }
        public string no_pol_lama { get; set; }
        public string nm_ttg { get; set; }
        public string nm_ttj { get; set; }
    }
}