using ABB.Application.Common.Interfaces;
using ABB.Application.RiskAndLossProfiles.Commands;
using ABB.Application.RiskAndLossProfiles.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RiskAndLossProfile.Models
{
    public class RiskAndLossProfileViewModel : IMapFrom<SaveRiskAndLossProfileCommand>
    {
        public string kd_cob { get; set; }

        public int nomor { get; set; }

        public decimal bts1 { get; set; }
        
        public decimal bts2 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RiskAndLossProfileViewModel, SaveRiskAndLossProfileCommand>();
            profile.CreateMap<RiskAndLossProfileDto, RiskAndLossProfileViewModel>();
        }
    }
}