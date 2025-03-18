using System;

namespace ABB.Domain.Entities
{
    public class Peserta
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }

        public string nomor_sppa { get; set; }

        public string nomor_sert { get; set; }

        public string nomor_polis { get; set; }

        public string kd_approval { get; set; }

        public string flag_new_ttg { get; set; }

        public string kd_grp_ttg { get; set; }

        public string kd_jns_ttg { get; set; }

        public string kd_ttg { get; set; }

        public string nm_ttg { get; set; }

        public string almt_ttg { get; set; }

        public string jns_kelamin { get; set; }

        public string tmpt_lahir { get; set; }

        public DateTime? tgl_lahir { get; set; }

        public string email { get; set; }

        public string kode_pos { get; set; }

        public string qq { get; set; }

        public string no_ktp { get; set; }

        public string no_npwp { get; set; }

        public string no_telepon { get; set; }

        public int no_sumber_penghasilan { get; set; }

        public string sumber_penghasilan_lain { get; set; }

        public int no_pekerjaan { get; set; }

        public string pekerjaan_lain { get; set; }

        public int no_jabatan { get; set; }

        public string jabatan_lain { get; set; }

        public string tmpt_kerja { get; set; }

        public Int16 kd_status { get; set; }

        public DateTime? tgl_status { get; set; }

        public string kd_user_status { get; set; }

        public string remark_status { get; set; }

        public DateTime tgl_input { get; set; }

        public string kd_user_input { get; set; }

        public DateTime tgl_edit { get; set; }

        public string kd_user_edit { get; set; }

        public string flag_closing { get; set; }

        public string flag_posting { get; set; }

        public Int16 usia { get; set; }

        public string no_sert { get; set; }

        public string kd_thn_sert { get; set; }

        public Int16? st_confirm { get; set; }
    }
}