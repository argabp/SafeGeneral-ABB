
namespace ABB.Domain.Entities
{
    public class KlaimAlokasiReasuransiXL
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public short no_mts { get; set; }
        public string kd_jns_sor { get; set; }
        public string kd_kontr { get; set; }
        public decimal nilai_kl { get; set; }
        public decimal? nilai_reinst { get; set; }
    }
}
