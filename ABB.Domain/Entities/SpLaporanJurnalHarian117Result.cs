using System;

namespace ABB.Domain.Entities
{
    public class SpLaporanJurnalHarian117Result
    {
        public string GlBukti { get; set; }
        public DateTime? GlTanggal { get; set; }
        public string GlAkun { get; set; }
        public string GlMtu { get; set; }
        public decimal? GlNilaiOrg { get; set; }
        public decimal? GlNilaiIdr { get; set; }
        public string GlDk { get; set; }
        public string GlKet { get; set; }
    }
}