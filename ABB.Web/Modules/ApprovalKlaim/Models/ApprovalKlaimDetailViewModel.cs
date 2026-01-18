using System;
using ABB.Application.ApprovalKlaims.Commands;
using ABB.Application.ApprovalKlaims.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.ApprovalKlaim.Models
{
    public class ApprovalKlaimDetailViewModel : IMapFrom<AddApprovalKlaimDetailCommand>
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
            profile.CreateMap<ApprovalKlaimDetailViewModel, AddApprovalKlaimDetailCommand>();
            profile.CreateMap<ApprovalKlaimDetailViewModel, EditApprovalKlaimDetailCommand>();
            profile.CreateMap<ApprovalKlaimDetailDto, ApprovalKlaimDetailViewModel>();
        }
    }
}