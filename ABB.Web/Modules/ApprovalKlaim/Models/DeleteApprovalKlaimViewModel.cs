using ABB.Application.ApprovalKlaims.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.ApprovalKlaim.Models
{
    public class DeleteApprovalKlaimViewModel : IMapFrom<DeleteApprovalKlaimCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteApprovalKlaimViewModel, DeleteApprovalKlaimCommand>();
        }
    }
}