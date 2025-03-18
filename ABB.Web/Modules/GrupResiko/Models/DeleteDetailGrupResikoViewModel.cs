using ABB.Application.Common.Interfaces;
using ABB.Application.GrupResikos.Commands;
using AutoMapper;

namespace ABB.Web.Modules.GrupResiko.Models
{
    public class DeleteDetailGrupResikoViewModel : IMapFrom<DeleteDetailGrupResikoCommand>
    {
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailGrupResikoViewModel, DeleteDetailGrupResikoCommand>();
        }
    }
}