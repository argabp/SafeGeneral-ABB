using System;

namespace ABB.Domain.Entities
{
    public class DokumenAkseptasiDetil
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public bool flag_wajib { get; set; }
    }
}