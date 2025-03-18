using System;
using ABB.Application.Approval.Commands;
using ABB.Application.Common.Interfaces;
using ABB.Application.PengajuanAkseptasi.Commands;
using AutoMapper;

namespace ABB.Api.Dto
{
    public class DataAkseptasiDto : IMapFrom<DataAkseptasiNasLifeCommand>
    {
        public string kd_cb { get; set; }

        public string kd_product { get; set; }

        public string kd_thn { get; set; }

        public string kd_rk { get; set; }

        public string no_sppa { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_user_status { get; set; }

        public string kd_approval { get; set; }

        public string keterangan { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DataAkseptasiDto, DataAkseptasiNasLifeCommand>();
            profile.CreateMap<DataAkseptasiDto, ConfirmNasLifeCommand>();
        }
    }
}