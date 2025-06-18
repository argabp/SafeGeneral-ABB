using System;
using ABB.Application.Approvals.Commands;
using ABB.Application.Approvals.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Approval.Models
{
    public class ApprovalDetailViewModel : IMapFrom<AddApprovalDetailCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user_sign { get; set; }

        public Int16 sla { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalDetailViewModel, AddApprovalDetailCommand>();
            profile.CreateMap<ApprovalDetailViewModel, EditApprovalDetailCommand>();
            profile.CreateMap<ApprovalDetailDto, ApprovalDetailViewModel>();
        }
    }
}