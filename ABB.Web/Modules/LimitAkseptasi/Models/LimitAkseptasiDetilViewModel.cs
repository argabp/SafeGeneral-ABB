using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LimitAkseptasis.Commands;
using ABB.Application.LimitAkseptasis.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LimitAkseptasi.Models
{
    public class LimitAkseptasiDetilViewModel : IMapFrom<AddLimitAkseptasiDetilCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public int thn { get; set; }

        public string kd_user { get; set; }

        public decimal pst_limit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LimitAkseptasiDetilViewModel, AddLimitAkseptasiDetilCommand>();
            profile.CreateMap<LimitAkseptasiDetilViewModel, EditLimitAkseptasiDetilCommand>();
            profile.CreateMap<LimitAkseptasiDetilDto, LimitAkseptasiDetilViewModel>();
        }
    }
}