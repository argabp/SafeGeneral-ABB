using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.RenewalReminder.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RenewalReminder.Models
{
    public class RenewalReminderViewModel : IMapFrom<GetRenewalReminderQuery>
    {
        public DateTime kd_bln_mul { get; set; }
        
        public DateTime kd_bln_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RenewalReminderViewModel, GetRenewalReminderQuery>();
        }
    }
}