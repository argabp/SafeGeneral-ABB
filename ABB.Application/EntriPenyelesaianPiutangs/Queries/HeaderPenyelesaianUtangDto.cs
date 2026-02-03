using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPenyelesaianPiutangs.Queries
{
    public class HeaderPenyelesaianUtangDto : IMapFrom<HeaderPenyelesaianUtang>
    {
        public string KodeCabang { get; set; }
        public string JenisPenyelesaian { get; set; }
        public string NomorBukti { get; set; }
        public string KodeVoucherAcc { get; set; }
        public DateTime? Tanggal { get; set; }
        public string MataUang { get; set; }
        public decimal? TotalOrg { get; set; }
        public decimal? TotalRp { get; set; }
        public string DebetKredit { get; set; }
        public string Keterangan { get; set; }
        public string KodeAkun { get; set; }
        public DateTime? TanggalInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserUpdate { get; set; }
        public bool FlagPosting { get; set; }
        public bool FlagFinal { get; set; }
        public string DetailMataUang { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<HeaderPenyelesaianUtang, HeaderPenyelesaianUtangDto>();


        }
    }
}