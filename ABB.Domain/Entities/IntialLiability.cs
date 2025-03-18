using System;

namespace ABB.Domain.Entities
{
    public class IntialLiability
    {
        public Int64 Id { get; set; }

        public DateTime PeriodeProses { get; set; }

        public decimal BELclaim { get; set; }

        public decimal BELexpense { get; set; }

        public decimal RA { get; set; }

        public decimal CSM { get; set; }

        public decimal LRC { get; set; }
    }
}