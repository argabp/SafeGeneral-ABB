using ABB.Application.Common.Interfaces;
using ABB.Application.GrupObyeks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.GrupObyek.Models
{
    public class GrupObyekViewModel : IMapFrom<SaveGrupObyekCommand>
    {
        public string kd_grp_oby { get; set; }

        public string nm_grp_oby { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GrupObyekViewModel, SaveGrupObyekCommand>();
            profile.CreateMap<GrupObyekViewModel, DeleteGrupObyekCommand>();
        }
    }
}