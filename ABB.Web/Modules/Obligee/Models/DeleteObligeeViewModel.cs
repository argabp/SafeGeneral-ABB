using ABB.Application.Common.Interfaces;
using ABB.Application.Obligees.Commands;
using AutoMapper;

namespace ABB.Web.Modules.Obligee.Models
{
    public class DeleteObligeeViewModel : IMapFrom<DeleteObligeeCommand>
    {
        public string kd_cb { get; set; }

        public string kd_grp_rk { get; set; }

        public string kd_rk { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteObligeeViewModel, DeleteObligeeCommand>();
            profile.CreateMap<DeleteObligeeViewModel, DeleteDetailObligeeCommand>();
        }
    }
}