using System;
using System.Collections.Generic;
using ABB.Application.VoucherBanks.Queries;

namespace ABB.Web.Modules.ListVoucher.Models
{
    public class ListVoucherViewModel
    {
        public DateTime TanggalAwal { get; set; }
        public DateTime TanggalAkhir { get; set; }

        public string KodeBank { get; set; } 
        public List<VoucherBankDto> VoucherList { get; set; } = new List<VoucherBankDto>();
    }
}
