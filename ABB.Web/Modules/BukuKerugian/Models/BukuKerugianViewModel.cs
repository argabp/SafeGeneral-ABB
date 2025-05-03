using System;
using ABB.Application.BukuKerugian.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.BukuKerugian.Models
{
    public class BukuKerugianViewModel : IMapFrom<GetBukuKerugianQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }
        
        public DateTime kd_mul { get; set; }
        
        public DateTime kd_akh { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BukuKerugianViewModel, GetBukuKerugianQuery>();
        }
    }
}