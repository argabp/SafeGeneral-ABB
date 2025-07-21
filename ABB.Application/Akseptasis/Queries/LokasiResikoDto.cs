namespace ABB.Application.Akseptasis.Queries
{
    public class LokasiResikoDto
    {
        public string Id { get; set; }
        
        public string kd_pos { get; set; }

        public string kd_lok_rsk { get; set; }

        public string? gedung { get; set; }

        public string? alamat { get; set; }

        public string? kd_prop { get; set; }

        public string? kd_kab { get; set; }

        public string? kd_kec { get; set; }

        public string? kd_kel { get; set; }
    }
}