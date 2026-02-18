using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPembayaranBanks.Queries
{
    public class EntriPembayaranBankTempDto : IMapFrom<EntriPembayaranBankTemp>
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
        
       
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
       
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayar { get; set; }
        public string DebetKredit { get; set; }
        public string UserBayar { get; set; }
        public decimal? TotalDlmRupiah { get; set; }
        public decimal? Kurs { get; set; }


        public decimal TotalBayarCalculated { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPembayaranBank, EntriPembayaranBankTempDto>();
            // Tambahkan .ForMember jika nama properti berbeda
        }
    }
}