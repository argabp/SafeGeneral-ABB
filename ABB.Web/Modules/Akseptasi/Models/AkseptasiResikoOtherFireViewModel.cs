using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoOtherFireViewModel : IMapFrom<AkseptasiOtherFireDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public string? kd_zona { get; set; }

        public string kd_prop { get; set; }

        public string? kd_lok_rsk { get; set; }

        public string almt_rsk { get; set; }

        public string kt_rsk { get; set; }

        public string? kd_pos_rsk { get; set; }

        public string? kd_penerangan { get; set; }

        public string kd_okup { get; set; }

        public string kd_kls_konstr { get; set; }

        public string? kategori_gd { get; set; }

        public Int16? umur_gd { get; set; }

        public Int16? jml_lantai { get; set; }

        public string? ket_okup { get; set; }

        public string? no_pol_ttg { get; set; }

        public string? kd_kab { get; set; }

        public string? kd_kec { get; set; }

        public string? kd_kel { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiOtherFireDto, AkseptasiResikoOtherFireViewModel>();
            profile.CreateMap<AkseptasiResikoOtherFireViewModel, SaveAkseptasiOtherFireCommand>();
        }
    }
}