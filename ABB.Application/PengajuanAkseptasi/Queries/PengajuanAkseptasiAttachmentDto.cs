using System;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class PengajuanAkseptasiAttachmentDto
    {
        public string kd_cb { get; set; }
        
        public string kd_cob { get; set; }
        
        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }

        public string dokumenName { get; set; }

        public bool? flag_wajib { get; set; }
    }
}