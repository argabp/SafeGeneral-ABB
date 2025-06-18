using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LimitAkseptasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.LimitAkseptasi.Models
{
    public class DeleteLimitAkseptasiDetilViewModel : IMapFrom<DeleteLimitAkseptasiDetilCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_user { get; set; }

        public decimal nilai_limit_awal { get; set; }

        public decimal nilai_limit_akhir { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteLimitAkseptasiDetilViewModel, DeleteLimitAkseptasiDetilCommand>();
        }
    }
}