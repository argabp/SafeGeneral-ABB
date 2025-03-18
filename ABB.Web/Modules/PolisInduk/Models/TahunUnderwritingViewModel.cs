using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PolisInduks.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PolisInduk.Models
{
    public class TahunUnderwritingViewModel : IMapFrom<GetTahunUnderwritingQuery>
    {
        public DateTime tgl_mul_ptg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TahunUnderwritingViewModel, GetTahunUnderwritingQuery>();
        }
    }
}