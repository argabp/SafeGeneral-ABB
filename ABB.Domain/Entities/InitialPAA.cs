using System;

namespace ABB.Domain.Entities
{
    public class InitialPAA
    {
        public Int64 Id { get; set; }

        public DateTime PeriodeProses { get; set; }

        public decimal LRC { get; set; }

        public decimal LRCIDR { get; set; }
    }
}