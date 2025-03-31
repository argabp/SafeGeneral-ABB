using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class KeteranganEndorsmentViewModel : IMapFrom<AkseptasiDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public string no_endt { get; set; }

        public string nm_ttg { get; set; }

        public string? almt_ttg { get; set; }

        public string? kt_ttg { get; set; }
        
        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public string? ket_endt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiDto, KeteranganEndorsmentViewModel>();
            profile.CreateMap<KeteranganEndorsmentViewModel, SaveKeteranganEndorsmentCommand>();
        }
    }
}