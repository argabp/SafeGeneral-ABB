using ABB.Application.Approvals.Commands;
using ABB.Application.Approvals.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Approval.Models
{
    public class ApprovalViewModel : IMapFrom<AddApprovalCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_approval { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalViewModel, AddApprovalCommand>();
            profile.CreateMap<ApprovalViewModel, EditApprovalCommand>();
            profile.CreateMap<ApprovalDto, ApprovalViewModel>();
        }
    }
}