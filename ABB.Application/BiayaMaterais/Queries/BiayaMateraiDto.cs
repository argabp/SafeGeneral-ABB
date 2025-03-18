namespace ABB.Application.BiayaMaterais.Queries
{
    public class BiayaMateraiDto
    {
        public string Id { get; set; }
        
        public string kd_mtu { get; set; }

        public string nm_mtu { get; set; }

        public decimal nilai_prm_mul { get; set; }
        
        public decimal nilai_prm_akh { get; set; }

        public decimal nilai_bia_mat { get; set; }
    }
}