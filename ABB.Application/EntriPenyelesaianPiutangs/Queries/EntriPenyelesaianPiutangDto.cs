using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class EntriPenyelesaianPiutangDto : IMapFrom<EntriPenyelesaianPiutang>
    {
        public string NoBukti { get; set; }
        public int No { get; set; }
        
        public string KodeAkun { get; set; }
        public string FlagPembayaran { get; set; }
       
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalBayarOrg { get; set; }
        public decimal? TotalBayarRp { get; set; }
        public string DebetKredit { get; set; }
        public string UserBayar { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPenyelesaianPiutang, EntriPenyelesaianPiutangDto>();
            // Tambahkan .ForMember jika nama properti berbeda
            
        }
    }
}