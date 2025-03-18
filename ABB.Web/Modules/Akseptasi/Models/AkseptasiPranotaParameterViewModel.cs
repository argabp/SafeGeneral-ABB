using System;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiPranotaParameterViewModel : IMapFrom<GetAkseptasiPranotaQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string kd_mtu { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiPranotaParameterViewModel, GetAkseptasiPranotaQuery>();
        }
    }
}