using System;

namespace ABB.Domain.Entities
{
    public class InquiryObyekCIT
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }

        public DateTime tgl_kirim { get; set; }

        public string? ket_kirim { get; set; }

        public decimal nilai_kas { get; set; }

        public decimal nilai_srt { get; set; }

        public Int16? jml_wesel { get; set; }

        public string? kd_lok_asal { get; set; }

        public string? kd_lok_tujuan { get; set; }

        public string? no_pol_ttg { get; set; }

        public Int16? jarak { get; set; }

        public string? ket_tujuan { get; set; }

        public decimal? nilai_pa { get; set; }

        public decimal? pst_rate_pa { get; set; }

        public decimal? nilai_prm_pa { get; set; }

        public decimal? pst_rate { get; set; }
    }
}