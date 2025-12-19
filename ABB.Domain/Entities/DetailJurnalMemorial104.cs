using System;

namespace ABB.Domain.Entities
{
    public class DetailJurnalMemorial104
    {
        public string NoVoucher { get; set; }
        public string FlagPosting { get; set; }
        public int No { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRP { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRP { get; set; }

        public string UserBayar { get; set; }
        public DateTime? TanggalBayar { get; set; }

        public string KodeUserInput { get; set; }
        public DateTime? TanggalUserInput { get; set; }

        public string KodeUserUpdate { get; set; }
        public DateTime? TanggalUserUpdate { get; set; }
    }
}
