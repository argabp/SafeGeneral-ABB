using ABB.Application.Common.Interfaces;
using ABB.Application.LimitTreaties.Commands;
using ABB.Application.LimitTreaties.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LimitTreaty.Models
{
    public class LimitTreatyViewModel : IMapFrom<SaveLimitTreatyCommand>
    {
        public string kd_cob { get; set; }

        public string kd_tol { get; set; }

        public string? nm_tol { get; set; }

        public string? kd_sub_grp { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitTreatyViewModel, SaveLimitTreatyCommand>();
            profile.CreateMap<LimitTreatyDto, LimitTreatyViewModel>();
        }
    }
}