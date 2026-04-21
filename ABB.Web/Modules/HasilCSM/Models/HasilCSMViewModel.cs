using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.HasilCSM.Queries;
using AutoMapper;

namespace ABB.Web.Modules.HasilCSM.Models
{
    public class HasilCSMViewModel : IMapFrom<GetLiabilityPAAQuery>
    {
        public string JenisLaporan { get; set; }

        public string KodeMetode { get; set; }

        public DateTime? PeriodeMulai { get; set; }

        public DateTime PeriodeAkhir { get; set; }

        public string Pengukuran { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<HasilCSMViewModel, GetLiabilityPAAQuery>();
            profile.CreateMap<HasilCSMViewModel, GetLiabilityPAARekapQuery>();
            profile.CreateMap<HasilCSMViewModel, GetLiabilityGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetLiabilityGMMRekapQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialGMMRekapQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialPAAQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseIntialPAARekapQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentGMMQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentGMMRekapQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentPAAQuery>();
            profile.CreateMap<HasilCSMViewModel, GetReleaseSubsequentPAARekapQuery>();
        }
    }
}