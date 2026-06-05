using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ListingSpreadingOfRisks.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ListingSpreadingOfRisk.Models
{
    public class ListingSpreadingOfRiskViewModel : IMapFrom<ListingSpreadingOfRiskCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime tgl_mul { get; set; }

        public DateTime tgl_akh { get; set; }

        public string no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ListingSpreadingOfRiskViewModel, ListingSpreadingOfRiskCommand>();
        }
    }
}