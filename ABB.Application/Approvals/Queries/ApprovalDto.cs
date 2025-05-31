using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Approvals.Queries
{
    public class ApprovalDto : IMapFrom<Approval>
    {
        public string kd_cb { get; set; }

        public string nm_cb { get; set; }

        public string kd_cob { get; set; }

        public string nm_cob { get; set; }

        public string kd_scob { get; set; }

        public string nm_scob { get; set; }

        public string nm_approval { get; set; }

        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Approval, ApprovalDto>();
        }
    }
}