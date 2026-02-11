using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.InquiryNotaProduksis.Queries; // <-- Pastikan using ini ada
using ABB.Application.EntriPembayaranBanks.Queries;
using ABB.Application.EntriPembayaranKass.Queries;
using ABB.Application.EntriPenyelesaianPiutangs.Queries;
using AutoMapper;
using System;
using System.Collections.Generic;
namespace ABB.Web.Modules.InquiryNotaProduksi.Models
{
    public class InquiryNotaProduksiViewModel 
    {
      
        
        public InquiryNotaProduksiDto InquiryNotaProduksiHeader { get; set; }
        public int id { get; set; } 

        public List<EntriPembayaranBankDto> PembayaranBankList { get; set; }
        public List<EntriPembayaranKasDto> PembayaranKasList { get; set; }
        public List<HeaderPenyelesaianUtangDto> PembayaranPiutangList { get; set; }

        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }

        
    }
}