namespace ABB.Domain.Entities
{
    public class DokumenRegisterKlaim
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public string kd_dok { get; set; }

        public string? flag_dok { get; set; }

        public string? link_file { get; set; }

        public bool? flag_wajib { get; set; }
    }
}