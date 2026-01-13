using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using System;

namespace ABB.Application.JenisTransaksis.Queries
{
    public class JenisTransaksiDto : IMapFrom<ABB.Domain.Entities.JenisTransaksi>
    {
        public long Id { get; set; }
        public string kode { get; set; }
        public string nama { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ABB.Domain.Entities.JenisTransaksi, JenisTransaksiDto>();
        }
    }
}