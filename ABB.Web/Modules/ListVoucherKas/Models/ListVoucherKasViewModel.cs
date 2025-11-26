using System;
using System.Collections.Generic;
using ABB.Application.VoucherKass.Queries;

namespace ABB.Web.Modules.ListVoucherKas.Models
{
    public class ListVoucherKasViewModel
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }
        public List<VoucherKasDto> VoucherList { get; set; } = new List<VoucherKasDto>();
    }
}
