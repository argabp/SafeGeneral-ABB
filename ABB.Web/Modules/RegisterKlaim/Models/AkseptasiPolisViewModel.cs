using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.RegisterKlaims.Queries;
using AutoMapper;

namespace ABB.Web.Modules.RegisterKlaim.Models
{
    public class AkseptasiPolisViewModel : IMapFrom<GetTanggalDanBuktiLunasQuery>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_pol { get; set; }
        
        public Int16 no_updt { get; set; }

        public string no_kl { get; set; }
        
        public DateTime tgl_kej { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiPolisViewModel, GetTanggalDanBuktiLunasQuery>();
        }
    }
}