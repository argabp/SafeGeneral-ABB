using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.ReopenMutasiKlaims.Commands;
using AutoMapper;

namespace ABB.Web.Modules.ReopenMutasiKlaim.Models
{
    public class ReopenMutasiKlaimViewModel : IMapFrom<ReopenMutasiKlaimCommand>
    {
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public string kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReopenMutasiKlaimViewModel, ReopenMutasiKlaimCommand>();
        }
    }
}