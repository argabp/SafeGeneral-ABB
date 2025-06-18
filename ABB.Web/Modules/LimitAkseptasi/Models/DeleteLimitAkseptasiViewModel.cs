using ABB.Application.Common.Interfaces;
using ABB.Application.LimitAkseptasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LimitAkseptasi.Models
{
    public class DeleteLimitAkseptasiViewModel : IMapFrom<DeleteLimitAkseptasiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteLimitAkseptasiViewModel, DeleteLimitAkseptasiCommand>();
        }
    }
}