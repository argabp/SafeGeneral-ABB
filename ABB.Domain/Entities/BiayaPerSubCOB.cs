namespace ABB.Domain.Entities
{
    public class BiayaPerSubCOB
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }

        public decimal nilai_min_prm { get; set; }

        public decimal nilai_bia_pol { get; set; }

        public decimal nilai_bia_adm { get; set; }

        public decimal nilai_min_form { get; set; }

        public decimal nilai_maks_plafond { get; set; }
    }
}