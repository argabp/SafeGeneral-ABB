using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoObyekCISViewModel : IMapFrom<AkseptasiObyekCISDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }

        public DateTime tgl_oby { get; set; }

        public decimal nilai_saldo { get; set; }

        public string? no_pol_ttg { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiObyekCISDto, AkseptasiResikoObyekCISViewModel>();
            profile.CreateMap<AkseptasiResikoObyekCISViewModel, SaveAkseptasiObyekCISCommand>();
        }
    }
}