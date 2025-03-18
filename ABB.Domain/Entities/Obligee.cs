using System;

namespace ABB.Domain.Entities
{
    public class Obligee
    {
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }

        public string? nm_rk { get; set; }

        public string? almt { get; set; }

        public string? kt { get; set; }

        public string? kd_pos { get; set; }

        public string? no_tlp { get; set; }

        public string? no_fax { get; set; }

        public string? npwp { get; set; }

        public string? flag_sic { get; set; }

        public string no_ktp { get; set; }

        public string? person { get; set; }

        public string? person_tlp { get; set; }

        public string? person_tlp_rmh { get; set; }

        public string? person_tlp_ktr { get; set; }

        public string? jbt_person { get; set; }

        public string? kd_rk_ref { get; set; }

        public DateTime? tgl_berdiri { get; set; }

        public string? no_akta { get; set; }

        public string? no_rekening { get; set; }

        public string? nm_dirut { get; set; }

        public string? kd_rk_induk { get; set; }

        public string? kd_kota { get; set; }
    }
}