namespace ABB.Application.CetakRekapitulasiProduksiPremiFakultatifMasuks.Commands
{
    public class CetakRekapitulasiProduksiPremiFakultatifMasukModel
    {
        public string nm_cob_ing { get; set; }
        public decimal? nilai_prm { get; set; }
        public decimal? nilai_prm_bl { get; set; }
        public decimal? nilai_kms { get; set; }
        public decimal? nilai_kms_bl { get; set; }
        public string? nm_kpl_bag { get; set; }
        public string? nm_bag { get; set; }
    }
}