namespace ABB.Application.CetakRekapitulasiProduksiTreatyMasuks.Commands
{
    public class CetakRekapitulasiProduksiTreatyMasukModel
    {
        public string nm_cob_ing { get; set; }
        public decimal? nilai_prm { get; set; }
        public decimal? nilai_prm_bl { get; set; }
        public decimal? nilai_kms { get; set; }
        public decimal? nilai_kms_bl { get; set; }
        public decimal? nilai_kl { get; set; }
        public decimal? nilai_kl_bl { get; set; }
        public string? nm_rk { get; set; }

        public string? kd_rk_pas { get; set; }
    }
}