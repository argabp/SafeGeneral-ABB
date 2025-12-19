using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using System;

namespace ABB.Application.JurnalMemorial117.Queries
{
    public class JurnalMemorial117DetailDto : IMapFrom<JurnalMemorial117Detail>
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JurnalMemorial117Detail, JurnalMemorial117DetailDto>();
        }
    }
}