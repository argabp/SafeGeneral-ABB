using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoParameterViewModel : IMapFrom<GetAkseptasiOtherFireQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public decimal? pst_share { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherFireQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherMotorQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherCargoQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherBondingQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherPAQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherHullQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, GetAkseptasiOtherHoleInOneQuery>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherBondingCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherCargoCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherFireCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherHoleInOneCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherHullCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherMotorCommand>();
            profile.CreateMap<AkseptasiResikoParameterViewModel, DeleteOtherPACommand>();
        }
    }
}