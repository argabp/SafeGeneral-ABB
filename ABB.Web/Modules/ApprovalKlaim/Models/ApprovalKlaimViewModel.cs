using ABB.Application.ApprovalKlaims.Commands;
using ABB.Application.ApprovalKlaims.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.ApprovalKlaim.Models
{
    public class ApprovalKlaimViewModel : IMapFrom<AddApprovalKlaimCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_approval { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalKlaimViewModel, AddApprovalKlaimCommand>();
            profile.CreateMap<ApprovalKlaimViewModel, EditApprovalKlaimCommand>();
            profile.CreateMap<ApprovalKlaimDto, ApprovalKlaimViewModel>();
        }
    }
}