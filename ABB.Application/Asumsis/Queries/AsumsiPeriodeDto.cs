
using System;

namespace ABB.Application.Asumsis.Queries
{
    public class AsumsiPeriodeDto
    {
        public string Id { get; set; }
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }
    }
}