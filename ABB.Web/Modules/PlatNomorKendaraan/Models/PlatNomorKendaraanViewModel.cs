using ABB.Application.Common.Interfaces;
using ABB.Application.PlatNomorKendaraans.Commands;
using ABB.Application.PlatNomorKendaraans.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PlatNomorKendaraan.Models
{
    public class PlatNomorKendaraanViewModel : IMapFrom<SavePlatNomorKendaraanCommand>
    {
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string? kd_ref { get; set; }
        public string kd_ref1 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PlatNomorKendaraanViewModel, SavePlatNomorKendaraanCommand>();
            profile.CreateMap<PlatNomorKendaraanDto, PlatNomorKendaraanViewModel>();
        }
    }
}