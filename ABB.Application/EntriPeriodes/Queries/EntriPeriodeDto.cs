using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriPeriodes.Queries
{
    public class EntriPeriodeDto : IMapFrom<EntriPeriode>
    {
        public decimal ThnPrd { get; set; }
        public short BlnPrd { get; set; }
        public DateTime? TglMul { get; set; }
        public DateTime? TglAkh { get; set; }
        public string FlagClosing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPeriode, EntriPeriodeDto>();
        }
    }
}