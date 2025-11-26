using System;

namespace ABB.Application.ApprovalAkseptasis.Queries
{
    public class ApprovalAkseptasiDto
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }
        
        public string no_aks { get; set; }

        public string nm_scob { get; set; }

        public string nomor_pengajuan { get; set; }

        public string nm_tertanggung { get; set; }

        public string status { get; set; }

        public string user_status { get; set; }

        public DateTime tgl_status { get; set; }

        public string kd_user_status { get; set; }

        public bool? flag_approved { get; set; }
        public bool? flag_banding { get; set; }
        public string? flag_closing { get; set; }
    }
}