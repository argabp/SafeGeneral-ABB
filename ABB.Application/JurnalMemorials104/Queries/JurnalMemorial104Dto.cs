using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using System;

namespace ABB.Application.JurnalMemorials104.Queries
{
    public class JurnalMemorial104Dto : IMapFrom<ABB.Domain.Entities.JurnalMemorial104>
    {
        public string KodeCabang { get; set; }
        public string NoVoucher { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Keterangan { get; set; }
        public bool? FlagGL { get; set; }
        public DateTime? TanggalInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserUpdate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ABB.Domain.Entities.JurnalMemorial104, JurnalMemorial104Dto>();
        }
    }
}