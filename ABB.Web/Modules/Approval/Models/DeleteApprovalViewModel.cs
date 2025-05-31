using ABB.Application.Approvals.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Approval.Models
{
    public class DeleteApprovalViewModel : IMapFrom<DeleteApprovalCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteApprovalViewModel, DeleteApprovalCommand>();
        }
    }
}