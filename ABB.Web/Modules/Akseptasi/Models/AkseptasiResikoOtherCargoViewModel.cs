using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoOtherCargoViewModel : IMapFrom<AkseptasiOtherCargoDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string nm_kapal { get; set; }

        public string? grp_kond { get; set; }

        public string? kd_kond { get; set; }

        public DateTime? tgl_brkt { get; set; }

        public string? no_bl { get; set; }

        public string? no_lc { get; set; }

        public string? no_inv { get; set; }

        public string tempat_brkt { get; set; }

        public string? tempat_tiba { get; set; }

        public string? tempat_transit { get; set; }

        public string? consignee { get; set; }

        public string? kond_sps { get; set; }

        public string? survey { get; set; }

        public string? no_po { get; set; }

        public string? no_pol_ttg { get; set; }

        public string kd_kapal { get; set; }

        public string? link_file { get; set; }

        public string? st_transit { get; set; }

        public IFormFile file { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiOtherCargoDto, AkseptasiResikoOtherCargoViewModel>();
            profile.CreateMap<AkseptasiResikoOtherCargoViewModel, SaveAkseptasiOtherCargoCommand>();
        }
    }
}