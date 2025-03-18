using ABB.Application.COBs.Commands;
using ABB.Application.COBs.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.COB.Models
{
    public class COBViewModel : IMapFrom<COBDto>
    {
        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string nm_cob_ing { get; set; }

        public string kd_class { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<COBDto, COBViewModel>();
            profile.CreateMap<COBViewModel, AddCOBCommand>();
            profile.CreateMap<COBViewModel, EditCOBCommand>();
        }
    }
}