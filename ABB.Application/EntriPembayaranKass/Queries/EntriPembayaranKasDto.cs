using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPembayaranKass.Queries
{
    public class EntriPembayaranKasDto : IMapFrom<EntriPembayaranKas>
    {
        // Bagian dari Composite Primary Key
        public string NoVoucher { get; set; }
        // Bagian dari Composite Primary Key
        public int No { get; set; }

        public string FlagPosting { get; set; }
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
        public string DebetKredit { get; set; }
        public string NoNota4 { get; set; }
        public string KodeMataUang { get; set; }
        public int? TotalBayar { get; set; }
        public string UserBayar { get; set; }
        public decimal? TotalDlmRupiah { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPembayaranKas, EntriPembayaranKasDto>();
        }
    }
}