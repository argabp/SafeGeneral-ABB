using System;

namespace ABB.Domain.Entities
{
    public class NotaDinas
    {
        public int id_nds { get; set; }

        public string no_nds { get; set; }

        public string kd_cb { get; set; }

        public string kd_rk { get; set; }

        public DateTime tgl_nds { get; set; }

        public string Perihal { get; set; }

        public decimal prm_bruto { get; set; }

        public decimal kms { get; set; }

        public decimal pph { get; set; }

        public decimal prm_netto { get; set; }

        public Int16 kd_status { get; set; }

        public DateTime tgl_status { get; set; }

        public string kd_user_status { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_user_input { get; set; }

        public DateTime tgl_edit { get; set; }

        public string kd_user_edit { get; set; }

        public int? jml_peserta { get; set; }

        public string nomor_polis { get; set; }

        public decimal pst_kms { get; set; }

        public decimal pst_pph { get; set; }

        public string no_bukti_lunas { get; set; }

        public DateTime? tgl_lunas { get; set; }

        public string no_invoice { get; set; }
    }
}