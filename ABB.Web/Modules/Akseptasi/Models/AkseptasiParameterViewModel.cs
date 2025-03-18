using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiParameterViewModel : IMapFrom<GetAkseptasiQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherFireQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherMotorQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherCargoQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherBondingQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherPAQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherHullQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, GetAkseptasiOtherHoleInOneQuery>();
            profile.CreateMap<AkseptasiParameterViewModel, ClosingAkseptasiCommand>();
        }
    }
}