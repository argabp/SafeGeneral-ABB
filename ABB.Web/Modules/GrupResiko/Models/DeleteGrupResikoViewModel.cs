using ABB.Application.Common.Interfaces;
using ABB.Application.GrupResikos.Commands;
using AutoMapper;

namespace ABB.Web.Modules.GrupResiko.Models
{
    public class DeleteGrupResikoViewModel : IMapFrom<DeleteGrupResikoCommand>
    {
        public string kd_grp_rsk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteGrupResikoViewModel, DeleteGrupResikoCommand>();
        }
    }
}