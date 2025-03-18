using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PolisInduks.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PolisInduk.Models
{
    public class JangkaWaktuPertanggunganViewModel : IMapFrom<GetJangkaWaktuPertanggunganQuery>
    {
        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public string kd_cob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JangkaWaktuPertanggunganViewModel, GetJangkaWaktuPertanggunganQuery>();
        }
    }
}