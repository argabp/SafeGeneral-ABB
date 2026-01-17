using ABB.Domain.Entities;

namespace ABB.Application.TemplateLapKeus.Queries
{
    // Hapus inheritance IMapFrom
    public class TemplateLapKeuDto 
    {
        public long Id { get; set; }
        public string TipeLaporan { get; set; }
        public string TipeBaris { get; set; }
        public string Deskripsi { get; set; }
        public string Rumus { get; set; }
        public string Level { get; set; }
    }
}