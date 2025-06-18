using ABB.Application.Common.Interfaces;
using ABB.Application.LimitAkseptasis.Commands;
using ABB.Application.LimitAkseptasis.Quries;
using AutoMapper;

namespace ABB.Web.Modules.LimitAkseptasi.Models
{
    public class LimitAkseptasiViewModel : IMapFrom<AddLimitAkseptasiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitAkseptasiViewModel, AddLimitAkseptasiCommand>();
            profile.CreateMap<LimitAkseptasiViewModel, EditLimitAkseptasiCommand>();
            profile.CreateMap<LimitAkseptasiDto, LimitAkseptasiViewModel>();
        }
    }
}