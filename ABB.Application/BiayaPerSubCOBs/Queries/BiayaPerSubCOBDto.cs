namespace ABB.Application.BiayaPerSubCOBs.Queries
{
    public class BiayaPerSubCOBDto
    {
        public string Id { get; set; }
        
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_mtu { get; set; }

        public string nm_mtu { get; set; }

        public string nm_cob { get; set; }

        public string nm_scob { get; set; }

        public decimal nilai_min_prm { get; set; }

        public decimal nilai_bia_pol { get; set; }

        public decimal nilai_bia_adm { get; set; }

        public decimal nilai_min_form { get; set; }

        public decimal nilai_maks_plafond { get; set; }
    }
}