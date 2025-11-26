using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.MutasiKlaims.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.MutasiKlaim.Models
{
    public class MutasiKlaimAlokasiViewModel : IMapFrom<SaveMutasiKlaimAlokasiCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public string kd_grp_pas { get; set; }
        
        public string kd_rk_pas { get; set; }

        public decimal pst_share { get; set; }
        
        public decimal nilai_kl { get; set; }

        public bool IsEdit { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MutasiKlaimAlokasiViewModel, SaveMutasiKlaimAlokasiCommand>();
            profile.CreateMap<MutasiKlaimAlokasi, MutasiKlaimAlokasiViewModel>();
        }
    }
}