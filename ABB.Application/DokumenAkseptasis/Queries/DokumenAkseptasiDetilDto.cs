using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.DokumenAkseptasis.Queries
{
    public class DokumenAkseptasiDetilDto : IMapFrom<DokumenAkseptasiDetil>
    {
        public int Id { get; set; }
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_dokumen { get; set; }

        public string nm_dokumen { get; set; }

        public bool flag_wajib { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DokumenAkseptasiDetil, DokumenAkseptasiDetilDto>();
        }
    }
}