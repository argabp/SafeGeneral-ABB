using System;

namespace ABB.Domain.Entities
{
    public class SubsequentPAA
    {
        public Int64 Id { get; set; }

        public DateTime PeriodeProses { get; set; }

        public decimal LRCRelease { get; set; }

        public decimal LRCReleaseIDR { get; set; }

        public decimal LRCReleaseMovement { get; set; }

        public decimal LRCReleaseMovementIDR { get; set; }
    }
}