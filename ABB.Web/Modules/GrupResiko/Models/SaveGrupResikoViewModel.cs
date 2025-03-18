using ABB.Application.Common.Interfaces;
using ABB.Application.GrupResikos.Commands;
using AutoMapper;

namespace ABB.Web.Modules.GrupResiko.Models
{
    public class SaveGrupResikoViewModel : IMapFrom<AddGrupResikoCommand>
    {
        public string kd_grp_rsk { get; set; }

        public string desk_grp_rsk { get; set; }

        public string kd_jns_grp { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SaveGrupResikoViewModel, AddGrupResikoCommand>();
            profile.CreateMap<SaveGrupResikoViewModel, EditGrupResikoCommand>();
        }
    }
}