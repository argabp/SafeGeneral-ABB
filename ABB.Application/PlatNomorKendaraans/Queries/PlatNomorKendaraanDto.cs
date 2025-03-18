namespace ABB.Application.PlatNomorKendaraans.Queries
{
    public class PlatNomorKendaraanDto
    {
        public string Id { get; set; }
        
        public string kd_grp_rsk { get; set; }

        public string desk_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string? kd_ref { get; set; }
        
        public string? kd_ref1 { get; set; }

        public string nm_ref { get; set; }
    }
}