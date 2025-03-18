using System;

namespace ABB.Application.PertanggunganKendaraans.Queries
{
    public class PertanggunganKendaraanDto
    {
        public string Id { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public string? nm_cob { get; set; }

        public string? desk { get; set; }

        public Int16? jml_hari { get; set; }

        public string? ket_klasula { get; set; }

        public string? flag_tjh { get; set; }

        public string? flag_rscc { get; set; }

        public string? flag_banjir { get; set; }

        public string? flag_accessories { get; set; }

        public string? flag_lain_lain01 { get; set; }
        
        public string? flag_lain_lain02 { get; set; }
    }
}