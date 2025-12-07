using System;

namespace ABB.Domain.Entities
{
    public class ViewTrKlaim
    {
        public string? kd_cb { get; set; }
        
        public string? kd_cob { get; set; }
        
        public string? kd_scob { get; set; }

        public string? kd_thn { get; set; }

        public string? no_kl { get; set; }
        
        public Int16? no_mts { get; set; }

        public string? nm_cb { get; set; }

        public string? nm_cob { get; set; }

        public string? nm_scob { get; set; }

        public string? nomor_berkas { get; set; }

        public string? nm_tertanggung { get; set; }

        public string? status { get; set; }

        public string? user_status { get; set; }

        public DateTime? tgl_reg { get; set; }
        
        public DateTime? tgl_status { get; set; }

        public string? ket_status { get; set; }

        public string kd_usr_input { get; set; }
        public string? kd_user_status { get; set; }

        public bool? flag_approved { get; set; }

        public string? flag_closing { get; set; }
    }
}