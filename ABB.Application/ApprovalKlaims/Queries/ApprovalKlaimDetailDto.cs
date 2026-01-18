using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.ApprovalKlaims.Queries
{
    public class ApprovalKlaimDetailDto  : IMapFrom<ApprovalKlaimDetail>
    {
        public int Id { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public string nm_user { get; set; }

        public Int16 kd_status { get; set; }

        public string kd_user_sign { get; set; }

        public string nm_user_sign { get; set; }

        public Int16 sla { get; set; }

        public string nm_status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApprovalKlaimDetail, ApprovalKlaimDetailDto >();
        }
    }
}