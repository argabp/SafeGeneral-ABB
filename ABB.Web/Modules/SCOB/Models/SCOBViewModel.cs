using ABB.Application.Common.Interfaces;
using ABB.Application.SCOBs.Commands;
using ABB.Application.SCOBs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.SCOB.Models
{
    public class SCOBViewModel : IMapFrom<SCOBDto>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_scob_ing { get; set; }

        public string kd_map_scob { get; set; }

        public string kd_sub_grp { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SCOBDto, SCOBViewModel>();
            profile.CreateMap<SCOBViewModel, AddSCOBCommand>();
            profile.CreateMap<SCOBViewModel, EditSCOBCommand>();
        }
    }
}