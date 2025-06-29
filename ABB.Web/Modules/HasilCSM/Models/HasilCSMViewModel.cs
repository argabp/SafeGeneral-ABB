using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.HasilCSM.Queries;
using AutoMapper;

namespace ABB.Web.Modules.HasilCSM.Models
{
    public class HasilCSMViewModel : IMapFrom<GetLiabilityPAAQuery>
    {
        public string TipeTransaksi { get; set; }

        public string KodeMetode { get; set; }

        public DateTime Periode { get; set; }

        public string Perhitungan { get; set; }

        public string Pengukuran { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<HasilCSMViewModel, GetLiabilityPAAQuery>();
            profile.CreateMap<HasilCSMViewModel, GetLiabilityGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialPAAQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentPAAQuery>();
        }
    }
}