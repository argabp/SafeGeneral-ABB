using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using System;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class JurnalMemorial104DetailDto : IMapFrom<DetailJurnalMemorial104>
    {
        public string NoVoucher { get; set; }
        public int No { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }
         public string KeteranganDetail { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailJurnalMemorial104, JurnalMemorial104DetailDto>();
        }
    }
}