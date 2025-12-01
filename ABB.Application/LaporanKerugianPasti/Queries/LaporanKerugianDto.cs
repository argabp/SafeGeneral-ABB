using System;

namespace ABB.Application.LaporanKerugianPasti.Queries
{
    public class LaporanKerugianDto
    {
        
        public string Id { get; set; }
        public string nm_cb { get; set; }
        
        public string nm_cob { get; set; }
        public string nm_scob { get; set; }
        public string nm_ttg { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public string tipe_mts { get; set; }
        public string nm_tipe_mts { get; set; }
        public string ket_jns { get; set; }

        public string nomor_berkas { get; set; }

        public string? no_pol_lama { get; set; }

        public DateTime? tgl_mts { get; set; }

        public DateTime? tgl_closing { get; set; }
    }
}