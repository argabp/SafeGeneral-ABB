using System;

namespace ABB.Application.Asumsis.Queries
{
    public class AsumsiDetailDto
    {
        public string Id { get; set; }
        public string KodeAsumsi { get; set; }

        public string KodeProduk { get; set; }

        public DateTime PeriodeProses { get; set; }

        public Int16 Thn { get; set; }

        public decimal Persentase { get; set; }
    }
}