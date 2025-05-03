using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PenyelesaianKlaim.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PenyelesaianKlaim.Models
{
    public class PenyelesaianKlaimViewModel : IMapFrom<GetPenyelesaianKlaimQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PenyelesaianKlaimViewModel, GetPenyelesaianKlaimQuery>();
        }
    }
}