using System;

namespace ABB.Domain.Entities
{
    public class Subsequent
    {
        public Int64 Id { get; set; }

        public DateTime PeriodeProses { get; set; }

        public decimal BELclaimliability { get; set; }

        public decimal BELclaimRelease { get; set; }

        public decimal UnwindingClaim { get; set; }

        public decimal BELclaimCurrent { get; set; }

        public decimal OCIBELclaim { get; set; }

        public decimal BELexpenseLiability { get; set; }

        public decimal BELexpenseRelease { get; set; }

        public decimal UnwindingExpense { get; set; }

        public decimal BELexpenseCurrent { get; set; }

        public decimal OCIBELexpense { get; set; }

        public decimal BELliability { get; set; }

        public decimal BELrelease { get; set; }

        public decimal UnwindingBEL { get; set; }

        public decimal RALiability { get; set; }

        public decimal RARelease { get; set; }

        public decimal UnwindingRA { get; set; }

        public decimal RACurrent { get; set; }

        public decimal OCIRA { get; set; }

        public decimal CSMLiability { get; set; }

        public decimal CSMRelease { get; set; }

        public decimal UnwindingCSM { get; set; }

        public decimal CSMRelease2 { get; set; }

        public decimal TotalCSMRelease { get; set; }

        public decimal LRCliability { get; set; }

        public decimal LRCRelease { get; set; }

        public decimal BELreleaseIDR { get; set; }

        public decimal RAReleaseIDR { get; set; }

        public decimal CSMReleaseIDR { get; set; }

        public decimal LRCReleaseIDR { get; set; }

        public decimal BELreleaseMovement { get; set; }

        public decimal RAReleaseMovement { get; set; }

        public decimal CSMReleaseMovement { get; set; }

        public decimal LRCReleaseMovement { get; set; }

        public decimal BELreleaseMovementIDR { get; set; }

        public decimal RAReleaseMovementIDR { get; set; }

        public decimal CSMReleaseMovementIDR { get; set; }

        public decimal LRCReleaseMovementIDR { get; set; }

        public decimal BELclaimReleaseIDR { get; set; }

        public decimal BELexpenseReleaseIDR { get; set; }

        public decimal BELclaimReleaseMovement { get; set; }

        public decimal BELexpenseReleaseMovement { get; set; }

        public decimal BELclaimReleaseMovementIDR { get; set; }

        public decimal BELexpenseReleaseMovementIDR { get; set; }

        public decimal komisiRelease { get; set; }

        public decimal komisiReleaseIDR { get; set; }

        public decimal komisiReleaseMovement { get; set; }

        public decimal komisiReleaseMovementIDR { get; set; }
    }
}