using System;

namespace ABB.Application.PengajuanAkseptasi.Queries
{
    public class ReportKeteranganPengajuanAkseptasiDto
    {
        public string? kd_cb { get; set; }

        public string? kd_cob { get; set; }

        public string? kd_scob { get; set; }

        public string? kd_thn { get; set; }

        public string? no_aks { get; set; }

        public string? nomor_pengajuan { get; set; }

        public Int16? no_urut { get; set; }

        public string? nm_user { get; set; }

        public DateTime? tgl_status { get; set; }

        public string? nm_status { get; set; }

        public string? ket_status { get; set; }
    }
}