using System;
using ABB.Application.ApprovalKlaims.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.ApprovalKlaim.Models
{
    public class DeleteApprovalKlaimDetailViewModel : IMapFrom<DeleteApprovalKlaimDetailCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user { get; set; }

        public string kd_user_sign { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteApprovalKlaimDetailViewModel, DeleteApprovalKlaimDetailCommand>();
        }
    }
}