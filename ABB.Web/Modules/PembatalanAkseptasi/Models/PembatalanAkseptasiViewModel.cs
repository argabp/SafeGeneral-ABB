using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PembatalanAkseptasis.Commands;
using AutoMapper;

namespace ABB.Web.Modules.PembatalanAkseptasi.Models
{
    public class PembatalanAkseptasiViewModel : IMapFrom<BatalAkseptasiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PembatalanAkseptasiViewModel, BatalAkseptasiCommand>();
        }
    }
}