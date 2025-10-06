using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    public class EntriPembayaranBankDto : IMapFrom<EntriPembayaranBank>
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
        
       
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
       
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
        public int? TotalBayar { get; set; }
        public string DebetKredit { get; set; }
        public string UserBayar { get; set; }
        public decimal? TotalDlmRupiah { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPembayaranBank, EntriPembayaranBankDto>();
            // Tambahkan .ForMember jika nama properti berbeda
        }
    }
}