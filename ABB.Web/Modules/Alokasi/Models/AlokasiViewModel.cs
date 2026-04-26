using System;
using ABB.Application.Alokasis.Commands;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Alokasi.Models
{
    public class AlokasiViewModel : IMapFrom<Domain.Entities.Alokasi>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }

        public short no_updt { get; set; }

        public short no_rsk { get; set; }

        public string kd_endt { get; set; }

        public short no_updt_reas { get; set; }

        public string kd_mtu_prm { get; set; }

        public decimal nilai_prm { get; set; }

        public decimal nilai_ttl_ptg { get; set; }

        public string flag_closing { get; set; }

        public DateTime tgl_closing { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Alokasi, AlokasiViewModel>();
            profile.CreateMap<AlokasiViewModel, SaveAlokasiCommand>();
        }
    }
}