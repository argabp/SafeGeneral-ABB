using System.Collections.Generic;
using ABB.Application.EntriPembayaranBanks.Queries;
using ABB.Application.EntriPembayaranKass.Queries;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class InquiryNotaProduksiPembayaranDto
    {
        public InquiryNotaProduksiDto Header { get; set; }

        public List<EntriPembayaranBankDto> PembayaranBank { get; set; }
            = new List<EntriPembayaranBankDto>();

        public List<EntriPembayaranKasDto> PembayaranKas { get; set; }
            = new List<EntriPembayaranKasDto>();

        public List<HeaderPenyelesaianUtangDto> PembayaranPiutang { get; set; }
            = new List<HeaderPenyelesaianUtangDto>();
    }
}
