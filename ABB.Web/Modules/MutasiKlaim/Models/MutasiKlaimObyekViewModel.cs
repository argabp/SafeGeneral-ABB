using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.MutasiKlaims.Commands;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Web.Modules.MutasiKlaim.Models
{
    public class MutasiKlaimObyekViewModel : IMapFrom<SaveMutasiKlaimObyekCommand>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_kl { get; set; }

        public Int16 no_mts { get; set; }

        public Int16 no_oby { get; set; }

        public string nm_oby { get; set; }

        public decimal nilai_kl { get; set; }

        public Int16? no_rsk { get; set; }

        public decimal? nilai_ttl_ptg { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MutasiKlaimObyekViewModel, SaveMutasiKlaimObyekCommand>();
            profile.CreateMap<MutasiKlaimObyek, MutasiKlaimObyekViewModel>();
        }
    }
}