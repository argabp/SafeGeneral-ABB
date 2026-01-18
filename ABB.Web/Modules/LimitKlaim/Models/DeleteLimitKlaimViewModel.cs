using ABB.Application.Common.Interfaces;
using ABB.Application.LimitKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LimitKlaim.Models
{
    public class DeleteLimitKlaimViewModel : IMapFrom<DeleteLimitKlaimCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteLimitKlaimViewModel, DeleteLimitKlaimCommand>();
        }
    }
}