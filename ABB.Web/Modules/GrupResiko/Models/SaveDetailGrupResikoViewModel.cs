using ABB.Application.Common.Interfaces;
using ABB.Application.GrupResikos.Commands;
using AutoMapper;

namespace ABB.Web.Modules.GrupResiko.Models
{
    public class SaveDetailGrupResikoViewModel : IMapFrom<AddDetailGrupResikoCommand>
    {
        
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string kd_ref { get; set; }

        public string kd_ref1 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveDetailGrupResikoViewModel, AddDetailGrupResikoCommand>();
            profile.CreateMap<SaveDetailGrupResikoViewModel, EditDetailGrupResikoCommand>();
        }
    }
}